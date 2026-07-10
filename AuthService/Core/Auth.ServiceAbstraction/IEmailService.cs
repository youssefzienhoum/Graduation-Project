using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceAbstraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body , IList<IFormFile> attachment = null);
    }
}
