using AutoMapper;
using Chat.Client.Grpc.ServiceAbstraction;
using Chat.Domain.Contracts;
using Chat.Domain.Entites;
using Chat.ServiceAbstraction;
using Chat.Shared.DTOS.Conversation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service
{
    public class ConversationService(
        IUnitOfWork unitOfWork, IMapper mapper,
        IHttpContextAccessor httpContextAccesso,
       IUserGrpcClient userGrpc ) : IConversationService
    {
        public async Task<CloseConversationResponse> CloseConversationAsync(Guid conversationId, CancellationToken cancellationToken = default)
        {
            var conversation = await unitOfWork
               .GetRepository<Conversation, Guid>()
                  .GetByIdAsync(
               new Specifications.ConversationSpecifications
                  .ConversationByIdSpecification(conversationId));

            if (conversation is null)
                throw new KeyNotFoundException("Conversation not found");

            conversation.Status = ConversationStatus.Closed;
            conversation.ClosedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CloseConversationResponse>(conversation);


        }

        public Task<ConversationResponse> CreateConversationAsync(Guid issueid, CreateConversationRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();

        }

        public async Task<ConversationResponse> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken = default)
        {
            var conversation = await unitOfWork
       .GetRepository<Conversation, Guid>()
       .GetByIdAsync(
           new Specifications.ConversationSpecifications
               .ConversationByIdSpecification(conversationId));

            if (conversation is null)
                throw new KeyNotFoundException("Conversation not found");

            return mapper.Map<ConversationResponse>(conversation);

        }

        public Task<IEnumerable<ConversationResponse>> GetConversationsByUserIdAsync(CancellationToken cancellationToken = default)
        {
            var userId = GetLoggedInUserId();
            var conversations = unitOfWork
                .GetRepository<Conversation, Guid>()
                .GetAllAsync(
                    new Specifications.ConversationSpecifications
                        .ConversationsByUserIdSpecification(userId));
            if (conversations is null)
                throw new KeyNotFoundException("No conversations found for the user.");
            return Task.FromResult(mapper.Map<IEnumerable<ConversationResponse>>(conversations));
        }

        public Task<ConversationResponse> UpdateConversationStatusAsync(Guid conversationId, string status, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private Guid GetLoggedInUserId()
        {
            var user = httpContextAccesso.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException(
                    "User not authenticated.");

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var id))
                throw new UnauthorizedAccessException(
                    "Invalid User Id.");

            return id;
        }
    }
}
