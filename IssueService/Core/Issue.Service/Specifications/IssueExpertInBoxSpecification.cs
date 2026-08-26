using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications
{
    internal class IssueExpertInBoxSpecification:BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public IssueExpertInBoxSpecification():base(null!) {
            
        
        }

        public IssueExpertInBoxSpecification(Guid id) : base(p=>p.Id==id)
        {
            AddInclude(p => p.AiAnalyses);
            AddInclude(p => p.GPSLocation);
            AddInclude(p => p.IssueAttachments);
            AddInclude(p=>p.ExpertReviews);
        }
    }
}
