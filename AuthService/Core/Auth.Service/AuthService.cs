using Auth.Domain.Contractts;
using Auth.Domain.Entities;
using Auth.ServiceAbstraction;
using Auth.Shared.DTOS.Auth;
using Auth.Shared.DTOS.OTP;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.TwiML.Messaging;
using Twilio.Types;
using static System.Net.WebRequestMethods;

namespace Auth.Service
{
    public class AuthService(
    UserManager<AppUser> userManager
    , IOTPService otpService
     , ITokenService tokenService
        , IEmailService _emailsender) : IAuthService
    {
        public async Task ForgetPasswordasync(ForgetPassowrdDto passowrdDto)
        {
            var user = await userManager.FindByEmailAsync(passowrdDto.Email!);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?> 
            { 
                {"token", token }, 
                { "email", passowrdDto.Email! } 
            };
            var callBack = QueryHelpers.AddQueryString(passowrdDto.ClinetUrl!, param);
            var message = new Message(user.Email, "Reset Password", callBack,null);
            await _emailsender.SendEmailAsync(user.Email!, "Reset Password", callBack, null!);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email!);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.token!, resetPasswordDto.Password!);
            if (!result.Succeeded)
            {
                throw new Exception("Password reset failed");
            }

        }
        public async Task<OTPResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == loginRequest.PhoneNumber);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await otpService.SendOtpAsync(loginRequest.PhoneNumber);
            return result;

        }

        public async Task<LoginWithEmailResponse> LoginWithEmailAsync(LoginWithEmail loginWithEmail)
        {
            
            var user = await userManager.FindByEmailAsync(loginWithEmail.Email!);
            if (user == null)
                throw new Exception("invalid email or password");
            //var result = await userManager.CheckPasswordAsync(user, LoginWithEmail.Password);
            //if (!result)
            //    throw new Exception("invalid email or password");
            var refreshtoken = await tokenService.CreateRefreshTokenAsync(user.Id);
            var accessToken = tokenService.GenerateAccessToken(user, await userManager.GetRolesAsync(user));
            return new LoginWithEmailResponse(
                FulltName : user.FulltName, refreshToken: refreshtoken.Token,accessToken:accessToken ,email :user.Email
            );
        }

        public async Task LogoutAsync(string refreshToken)
        {
            await tokenService.RevokeRefreshTokenAsync(refreshToken);

        }

        public async Task<OTPResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var userExists = await userManager.Users.AnyAsync(x => x.PhoneNumber == registerRequest.PhoneNumber);

            if (userExists)
            {
                throw new Exception("User already exists");
            }

            var user = new AppUser
            {
                FulltName = registerRequest.FullName,
                UserName = registerRequest.FullName,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.email,
                Address = new Address
                {
                    Village = registerRequest.village,
                    Region = registerRequest.Region
                },
                pictures = registerRequest.picture ?? string.Empty
            };


            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }
            await userManager.AddToRoleAsync(user, "Farmer");

            var otp = await otpService.SendOtpAsync(registerRequest.PhoneNumber);
            return otp;
        }

        public async Task<UserResponse> VerifyOTPAsync(VerifyOTPRequest verifyOTPRequest)
        {
            var result = await otpService.ValidateOtpAsync(verifyOTPRequest);
            if (!result)
            {
                throw new Exception("Invalid OTP");
            }

            var user = userManager.Users.FirstOrDefault(x => x.PhoneNumber == verifyOTPRequest.phonenumber);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Any())
            {
                await userManager.AddToRoleAsync(user, "Farmer");
            }

            var refreshtoken = await tokenService.CreateRefreshTokenAsync(user.Id);


            var accesstoken = tokenService.GenerateAccessToken(user, roles);

            var userResponse = new UserResponse(
             accesstoken: accesstoken,
        refershtoken: refreshtoken.Token,
         FullName: user.FulltName,
        message: "OTP verified successfully"
    );

            return userResponse;
        }
    }
}
