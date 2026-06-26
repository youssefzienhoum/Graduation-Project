using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Contract;
using User.Domain.Entities;
using User.ServicesAbstract;
using User.shared.DTOS;


namespace User.Services.Services
{
    internal class UserService(IUserRepo userRepo , IHttpContextAccessor httpContextAccesso, IMapper mapper) : IUserService
    {
        public async Task blockUserAsync(Guid userId)
        {
           var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.IsActive = false;
                await  userRepo.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
           var user=await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            await userRepo.DeleteAsync(user.Id);


        }

        public Task<IEnumerable<UserDetailsRespones>> GetAllUserDetailsAsync()
        {
            var users = userRepo.GetAllAsync().Result;
            return Task.FromResult(mapper.Map<IEnumerable<UserDetailsRespones>>(users));

        }

        public Task<UserDetailsRespones> GetUserDetailsAsync()
        {
            var user = GetLoggedInUserAsync().Result;
            return Task.FromResult(mapper.Map<UserDetailsRespones>(user));
        }

        public Task unblockUserAsync(Guid userId)
        {
            var user = userRepo.GetByIdAsync(userId).Result;
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.IsActive = true;
            return userRepo.UpdateAsync(user);
        }

        public Task UpdateUserDetailsAsync(UserUpdateRequest userUpdate)
        {
            var user = GetLoggedInUserAsync().Result;
            if (user == null)
            {
                throw new Exception("User not found");
            }
            mapper.Map(userUpdate, user);
            return userRepo.UpdateAsync(user);

        }

        private async Task<AppUser> GetLoggedInUserAsync()
        {
            var user = httpContextAccesso.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User Id not found in token");
            }

            return await userRepo.GetByIdAsync(Guid.Parse(userId)) ?? throw new Exception("User not found");
        }

    }
}
