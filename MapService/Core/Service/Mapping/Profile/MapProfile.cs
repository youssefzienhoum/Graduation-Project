using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Issue.Domain.Entities.Issue;
using Map.Shared;

namespace Map.Service.Mapping.Profile
{
    internal class MapProfile: AutoMapper.Profile
    {
        public MapProfile() {

            CreateMap<Issue.Domain.Entities.Issue.Issue, MapResponseDto>()
                .ForMember(dest => dest.IssueId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Longitde,
                opt => opt.MapFrom(src => src.GPSLocation.Longitude))
            .ForMember(dest => dest.Latitude,
                opt => opt.MapFrom(src => src.GPSLocation.Latitude))
            .ForMember(dest => dest.priority,
                opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.PhotoUrl
            , opt => opt.MapFrom(src => src.IssueAttachments.Select(c => c.Url)))
            .ForMember(dest=> dest.CreatedAt,
            opt=> opt.MapFrom(scr=>scr.CreatedAt))
            .ForMember(dest=> dest.Status
            ,opt=>opt.MapFrom(scr=>scr.Status))
            .ForMember(dest=>dest.title,
            opt=>opt.MapFrom(scr=>scr.Title));

        } 

    }
}
