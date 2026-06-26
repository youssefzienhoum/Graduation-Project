using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.shared.DTOS;


namespace User.ServicesAbstract
{
    public interface IUserService
    {
        Task<UserDetailsRespones> GetUserDetailsAsync();
        Task<IEnumerable<UserDetailsRespones>> GetAllUserDetailsAsync();
        Task UpdateUserDetailsAsync(UserUpdateRequest userUpdate);
        Task DeleteUserAsync(Guid userId);
        Task blockUserAsync(Guid userId);
        Task unblockUserAsync(Guid userId);
    


    }
}
