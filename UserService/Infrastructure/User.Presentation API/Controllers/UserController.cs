using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.ServicesAbstract;
using User.shared.DTOS;

namespace User.Presentation_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class UserController(IUserService userService): ControllerBase
    {
        [HttpGet("details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            var userDetails = await userService.GetUserDetailsAsync();
            return Ok(userDetails);
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserUpdateRequest userUpdate)
        {
            await userService.UpdateUserDetailsAsync(userUpdate);
            return NoContent();
        }
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await userService.DeleteUserAsync(userId);
            return NoContent();
        }
        public async Task<IActionResult> BlockUser(Guid userId)
        {
            await userService.blockUserAsync(userId);
            return NoContent();
        }

        public async Task<IActionResult> UnblockUser(Guid userId)
        {
            await userService.unblockUserAsync(userId);
            return NoContent();
        }


      

    }
}
