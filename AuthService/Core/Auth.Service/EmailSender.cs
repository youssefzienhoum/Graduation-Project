using Auth.ServiceAbstraction;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace Auth.Service;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    public EmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
    //private readonly MailSettings _settings;

    //public EmailService(IOptions<MailSettings> settings)
    //{
    //    _settings = settings.Value;
    //}

    //public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    //{
    //    var message = new MimeMessage();

    //    message.From.Add(new MailboxAddress("My App", _settings.From));
    //    message.To.Add(MailboxAddress.Parse(toEmail));
    //    message.Subject = subject;

    //    message.Body = new TextPart("html")
    //    {
    //        Text = htmlMessage
    //    };

    //    using var smtp = new SmtpClient();

    //    await smtp.(
    //        _settings.SmtpServer,
    //        _settings.Port,
    //        SecureSocketOptions.StartTls);

    //    await smtp.AuthenticateAsync(
    //        _settings.Username,
    //        _settings.Password);

    //    await smtp.SendAsync(message);

    //    await smtp.DisconnectAsync(true);
    //}

    public Task SendEmailAsync(string toEmail, string subject, string body, IList<IFormFile> attachment = null)
    {
        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Email),
            Subject = subject
        };
        email.To.Add(MailboxAddress.Parse(toEmail));
        var builder = new BodyBuilder();
        if (attachment != null)
        {
            Byte[] fileBytes;
            foreach (var file in attachment)
            {
                if (file.Length > 0)
                {
                    using var stream = new MemoryStream();
                    file.CopyTo(stream);
                    fileBytes = stream.ToArray();
                    builder.Attachments.Add(file.FileName, stream.ToArray(), ContentType.Parse(file.ContentType));
                }
            }
        }
        builder.HtmlBody = body;
        email.Body = builder.ToMessageBody();
        email.From.Add(new MailboxAddress(_mailSettings.Username, _mailSettings.Email));
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        smtp.Connect(_mailSettings.SmtpServer, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
        smtp.SendAsync(email);
        smtp.Disconnect(true);
        smtp.Dispose();

    }
}
        
   




