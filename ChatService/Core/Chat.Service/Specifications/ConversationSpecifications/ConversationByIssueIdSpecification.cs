using Chat.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Specifications.ConversationSpecifications
{
    internal class ConversationByIssueIdSpecification
     : BaseSpecification<Conversation>
    {
        public ConversationByIssueIdSpecification(Guid issueId)
            : base(c => c.IssueId == issueId)
        {
            AddInclude(c => c.Participants);
        }
    }
}
