using Auth.Domain.Contractts;
using Auth.ServiceAbstraction;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.DependanceInjection
{
    public static  class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services )
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOTPService, OtpService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ITokenService, TokenService>();




            return services;
        }
    }
}
