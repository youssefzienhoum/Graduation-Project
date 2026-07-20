using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.shared.DTOS;
using Dashboard.Shared.Result;


namespace Dashboard.ServicesAbstract
{
    public interface IUserService
    {
        Task<Result<UserDetailsResponse>> GetUserDetailsAsync();
        Task<Result<IEnumerable<UserDetailsResponse>>> GetAllUserDetailsAsync();
        Task<Result> UpdateUserDetailsAsync(UserUpdateRequest userUpdate);
        Task<Result> DeleteUserAsync(Guid userId);
        Task<Result> BlockUserAsync(Guid userId);
        Task<Result> ApprovedUserAsync(Guid userId);

    


    }
}
