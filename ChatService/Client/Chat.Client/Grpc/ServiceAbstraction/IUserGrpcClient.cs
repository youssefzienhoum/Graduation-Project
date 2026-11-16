using Chat.Shared.DTOS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Grpc.ServiceAbstraction
{
    public  interface IUserGrpcClient
    {
        Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetUsersByIdsAsync(
  IEnumerable<Guid> userIds,
  CancellationToken ct = default);

    }
}
