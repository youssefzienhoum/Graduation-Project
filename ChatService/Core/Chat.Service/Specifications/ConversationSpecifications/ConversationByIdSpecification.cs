using Chat.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Specifications.ConversationSpecifications
{
    internal class ConversationByIdSpecification
    : BaseSpecification<Conversation>
    {
        public ConversationByIdSpecification(Guid conversationId)
            : base(c => (c.Id == conversationId) &&
                c.Status != ConversationStatus.Closed)
        {
            AddInclude(c => c.Participants);
        }
    }
}
