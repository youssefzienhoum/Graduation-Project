using AutoMapper;
using Map.Domain.Contarcts;
using Map.ServiceAbsraction;
using Map.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Service
{
    public class MapService(IIssueRepo issueRepo,IMapper mapper ) : IMapSerevice
    {
        public async Task<MapResponseDto> SearchForIssueInMapAsync(Guid IssueId, CancellationToken cancellationToken)
        {
            var issue = await issueRepo.GetByIdAsync(IssueId);

            if (issue == null)
            {
                throw new KeyNotFoundException("Issue not found");
            }

            var result = mapper.Map<MapResponseDto>(issue);

            return result;



        }

        public async Task<IEnumerable<MapResponseDto>> ShowIssueInMapAsync(CancellationToken cancellationToken)
        {
           var issues = await issueRepo.GetAllAsync();
            if(issues == null || !issues.Any())
            {
                throw new KeyNotFoundException("No issues found");
            }   
            var result = mapper.Map<IEnumerable<MapResponseDto>>(issues);
            return result;


        }
    }
}
