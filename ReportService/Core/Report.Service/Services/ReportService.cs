using AutoMapper;
using Microsoft.AspNetCore.Http;
using Refit;
using Report.Client.AbstructServices;
using Report.Domain.Contracts;
using Report.Domain.Entities.Report;
using Report.Service.Mapping;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.Services
{
    public class ReportService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IStorageClient storageClient,
        IAiVisionClient aiVisionClient) : IReportService
    {
        public async Task<ReportDetailsResponse> AnalyzeReportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var report = await unitOfWork.ReportRepo.GetByIdAsync(id)
               ?? throw new KeyNotFoundException("Report not found.");

            var photoAttachment = report.Attachments.FirstOrDefault(a => a.Type == ReportAttachmentType.Photo);
            if (photoAttachment is null)
            {
                throw new InvalidOperationException("This report has no photo to analyze.");
            }

        
            await using var photoStream = await storageClient.DownloadAsync(photoAttachment.Url);

            var fileName = photoAttachment.Url.Split('/').LastOrDefault() ?? "photo.jpg";
            var prediction = await aiVisionClient.PredictAsync(new StreamPart(photoStream, fileName));

            var severity = AiAnalysisMapper.MapSeverity(prediction.Severity);

            report.Analysis = new AiAnalysis
            {
                ProblemName = prediction.Problem,
                ProblemArabic = prediction.ProblemArabic,
                Confidence = prediction.Confidence,
                Severity = severity,
                Recommendation = prediction.Recommendation,
                Explanation = prediction.Explanation,
                RepairSteps = prediction.RepairSteps,
                ModelVersion = prediction.ModelVersion ?? string.Empty,
            };
            report.Status = ReportStatus.Analyzed;
            report.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.ReportRepo.UpdateAsync(report);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ReportDetailsResponse>(report);

        }

        public async Task<ReportDetailsResponse> CreateReportAsync(CreateReportRequest request, CancellationToken cancellationToken = default)
        {
            var reporterId = GetLoggedInUserId();

            var report = new Domain.Entities.Report.Report
            {
                Description = request.Description,
                ReporterId = reporterId,
            };

            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                report.Location = new GPSLocation
                {
                    Latitude = request.Latitude.Value,
                    Longitude = request.Longitude.Value,
                };
            }

            if (request.photo is not null)
            {
                await using var stream = request.photo.OpenReadStream();
                var uploadResult = await storageClient.UploadAsync(
                    new StreamPart(stream, request.photo.FileName, request.photo.ContentType),
                    "reportimage");

                report.Attachments.Add(new ReportAttachment
                {
                    Type = ReportAttachmentType.Photo,
                    Url = uploadResult.filePath,
                });
            }

            await unitOfWork.ReportRepo.AddAsync(report);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ReportDetailsResponse>(report);

        }

        public async Task DeleteReportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var report = await unitOfWork.ReportRepo.GetByIdAsync(id)
               ?? throw new KeyNotFoundException("Report not found.");
            foreach (var attachment in report.Attachments)
            {
                await storageClient.DeleteAsync(attachment.Url);
            }

            await unitOfWork.ReportRepo.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync(cancellationToken);


        }

        public async Task<IEnumerable<ReportDetailsResponse>> GetAllReportsAsync(CancellationToken cancellationToken = default)
        {
         var reports = await unitOfWork.ReportRepo.GetAllAsync();
            var response = mapper.Map<IEnumerable<ReportDetailsResponse>>(reports);
            return response;

        }

        public async Task<IEnumerable<ReportDetailsResponse>> GetMyReportsAsync(CancellationToken cancellationToken = default)
        {
            var reporterId = GetLoggedInUserId();
            var reports = await unitOfWork.ReportRepo.GetByReporterIdAsync(reporterId);
            return mapper.Map<IEnumerable<ReportDetailsResponse>>(reports);

        }

        public async Task<ReportDetailsResponse> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var report = await unitOfWork.ReportRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Report not found.");

            return mapper.Map<ReportDetailsResponse>(report);

        }


        private Guid GetLoggedInUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user is null || !user.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedId))
            {
                throw new UnauthorizedAccessException("User Id not found in token.");
            }

            return parsedId;
        }
    }
}
