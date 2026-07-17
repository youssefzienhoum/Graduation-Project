using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.shared.DTOS;


namespace Dashboard.ServicesAbstract
{
    public interface IUserService
    {
        Task<UserDetailsResponse> GetUserDetailsAsync();
        Task<IEnumerable<UserDetailsResponse>> GetAllUserDetailsAsync();
        Task UpdateUserDetailsAsync(UserUpdateRequest userUpdate);
        Task DeleteUserAsync(Guid userId);
        Task BlockUserAsync(Guid userId);
       
        Task ApprovedUserAsync(Guid userId);

    


    }
}
