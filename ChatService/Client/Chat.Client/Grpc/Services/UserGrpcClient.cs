using Chat.Client.Grpc.ServiceAbstraction;
using Chat.Shared.DTOS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Grpc;




namespace Chat.Client.Grpc.Services
{
    public class UserGrpcClient(UserGrpcService.UserGrpcServiceClient client) : IUserGrpcClient
    {
        public async Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetUsersByIdsAsync(IEnumerable<Guid> userIds, CancellationToken ct)
        {
            var request = new GetUsersByIdsRequest();

            request.UserIds.AddRange(
                userIds.Select(id => id.ToString()));

            var response = await client.GetUsersByIdsAsync(
                request,
                cancellationToken: ct);

            return response.Users.ToDictionary(
                user => Guid.Parse(user.Id),
                user => new UserInfoDto(
                    Guid.Parse(user.Id),
                    user.FullName,
                    user.ProfileImageUrl
                ));
        }
    }
}

