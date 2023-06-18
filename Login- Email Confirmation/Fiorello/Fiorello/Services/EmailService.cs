﻿using Fiorello.Helpers;
using Fiorello.Services.Interfaces;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using MimeKit.Text;
using MimeKit;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;

namespace Fiorello.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public void Send(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _emailSettings.FromAddress));
            email.To.Add(MailboxAddress.Parse(to)); 
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Username, _emailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
