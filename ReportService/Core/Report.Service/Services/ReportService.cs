using AutoMapper;
using Microsoft.AspNetCore.Http;
using Refit;
using Report.Client.AbstructServices;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
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
        IAiVisionClient aiVisionClient) : IIssueService
    {
        public async Task<IssueDetailsResponse> AnalyzeIssueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var issue = await unitOfWork.issueRepo.GetByIdAsync(id)
               ?? throw new KeyNotFoundException("Issue not found.");

            var photoAttachment = issue.IssueAttachments.FirstOrDefault(a => a.Type == IssueAttachmentType.Photo);
            if (photoAttachment is null)
            {
                throw new InvalidOperationException("This report has no photo to analyze.");
            }


            await using var photoStream = await storageClient.DownloadAsync(photoAttachment.Url);

            var fileName = photoAttachment.Url.Split('/').LastOrDefault() ?? "photo.jpg";
            var prediction = await aiVisionClient.PredictAsync(new StreamPart(photoStream, fileName));

            if (prediction.Status != "success")
            {
                throw new InvalidOperationException(
                    prediction.Message ?? "The vision service couldn't produce a confident diagnosis for this photo.");
            }

            report.Analysis = new AiAnalysis
            {
                ProblemName = prediction.ProblemCode ?? string.Empty,
                ProblemArabic = prediction.Problem,
                Confidence = AiAnalysisMapper.ParseConfidence(prediction.Confidence),
                Severity = AiAnalysisMapper.MapSeverity(prediction.Severity),
                Recommendation = prediction.Recommendation ?? string.Empty,
                Explanation = prediction.Explanation,
                RepairSteps = prediction.RepairSteps ?? new List<string>(),
                ModelVersion = string.Empty, // vision-service doesn't return this currently
            };
            report.Status = ReportStatus.Analyzed;
            report.UpdatedAt = DateTime.UtcNow;


            await unitOfWork.ReportRepo.UpdateAsync(report);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ReportDetailsResponse>(report);

        }

        public async Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default)
        {
            var reporterId = GetLoggedInUserId();

            var report = new
            {
                Description = request.Description,
                ReporterId = reporterId,
            };
            if (request.Longitude is not null && request.Latitude is not null)
            {
                report.Location = new GPSLocation
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
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
            return mapper.Map<CreateReportResponse>(report);


        }

       

        public async Task DeleteIssueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var issue = await unitOfWork.issueRepo.GetByIdAsync(id)
               ?? throw new KeyNotFoundException("Report not found.");
            var deleteTasks = issue.IssueAttachments
     .Select(a => storageClient.DeleteAsync(a.Url));

            await Task.WhenAll(deleteTasks);

            await unitOfWork.issueRepo.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync(cancellationToken);


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
