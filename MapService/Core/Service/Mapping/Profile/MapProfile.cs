using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
                opt => opt.MapFrom(src => src.Priority));
        } 

    }
}
