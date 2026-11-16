using Chat.Domain.Entites;
using Chat.Shared.DTOS.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.ServiceAbstraction
{
    public interface IConversationService
    {
       
        Task <ConversationResponse> CreateConversationAsync(Guid issueid ,CreateConversationRequest request, CancellationToken cancellationToken = default);
        Task<ConversationResponse> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken = default);
       Task<IEnumerable<ConversationResponse>> GetConversationsByUserIdAsync( CancellationToken cancellationToken = default);
        Task<ConversationResponse> UpdateConversationStatusAsync(Guid conversationId, string status, CancellationToken cancellationToken = default);
        Task <CloseConversationResponse> CloseConversationAsync(Guid conversationId, CancellationToken cancellationToken = default);

    }
}
