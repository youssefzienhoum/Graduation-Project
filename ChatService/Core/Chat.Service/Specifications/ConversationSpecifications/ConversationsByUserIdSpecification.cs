using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Domain.Entites;

namespace Chat.Service.Specifications.ConversationSpecifications
{
    internal class ConversationsByUserIdSpecification : BaseSpecification<Conversation>
    {
        public ConversationsByUserIdSpecification(Guid userId)
             : base(c =>
                c.Participants.Any(p => p.UserId == userId) &&
                c.Status != ConversationStatus.Closed)
        {
            AddInclude(c => c.Participants);
            AddOrderByDesc(c => c.StartedAt);
        }
    }
}
