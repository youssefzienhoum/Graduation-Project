using AutoMapper;
using Report.Domain.Entities.Report;
using Report.Shared.DTOS.Client;
using Report.Shared.DTOS.Report;
using System.Linq;

namespace Report.Service.Mapping.Profile
{
    public class ReportProfile : AutoMapper.Profile
    {
        public ReportProfile()
        {
            CreateMap<Report.Domain.Entities.Report.Report, ReportDetailsResponse>()
                .ForCtorParam("Id",
                    opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Status",
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForCtorParam("Latitude",
                    opt => opt.MapFrom(src => src.Location != null ? (string?)src.Location.Latitude : null))
                .ForCtorParam("Longitude",
                    opt => opt.MapFrom(src => src.Location != null ? (string?)src.Location.Longitude : null))
                .ForCtorParam("Attachments",
                    opt => opt.MapFrom(src => src.Attachments))
                .ForCtorParam("Analysis",
                    opt => opt.MapFrom(src => src.Analysis));

            CreateMap<ReportAttachment, ReportAttachmentResponse>()
                .ForCtorParam("Type",
                    opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<AiAnalysis, AiAnalysisResponse>()
                .ForCtorParam("Severity",
                    opt => opt.MapFrom(src => src.Severity.ToString()));
        }
    }
}