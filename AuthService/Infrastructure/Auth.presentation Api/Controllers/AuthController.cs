using Auth.ServiceAbstraction;
using Auth.Shared.DTOS.Auth;
using Auth.Shared.DTOS.OTP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auth.presentation_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            var result = await authService.RegisterAsync(registerRequest);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await authService.LoginAsync(loginRequest);
            return Ok(result);

        }
        [HttpPost("loginwithemail")]
        public async Task<IActionResult> LoginWithEmail([FromBody] LoginWithEmail loginWithEmail)
        {
            var result = await authService.LoginWithEmailAsync(loginWithEmail);
            return Ok(result);
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPassowrdDto forgetPassowrdDto)
        {
            await authService.ForgetPasswordasync(forgetPassowrdDto);
            return Ok();
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            await authService.ResetPasswordAsync(resetPasswordDto);
            return Ok();
        }
        [HttpPost("verify OTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequest verifyOTPRequest)
        {
            var result = await authService.VerifyOTPAsync(verifyOTPRequest);
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await authService.LogoutAsync(refreshToken);
            return Ok();
        }
    }
}
