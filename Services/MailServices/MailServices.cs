using Domain.OptionsConfiguration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Services.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.MailServices
{
    public class MailServices : IMailServices
    {
        private readonly IOptions<EamilSettings> _options;

        public MailServices(IOptions<EamilSettings> options)
        {
            _options = options;
        }

        public async Task<ResultServices> SendMail(string mailTo, string subject, string htmlBody, IList<IFormFile> attachments = null)
        {
            var result = new ResultServices();

            try
            {
                // Create the email message
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
                email.To.Add(MailboxAddress.Parse(mailTo));
                email.Subject = subject;

                // Add headers to make email look professional
                email.Headers.Add("X-Priority", "1"); // High priority
                email.Headers.Add("X-MSMail-Priority", "High");
                email.Headers.Add("Importance", "High");
                email.Headers.Add("Reply-To", _options.Value.Email);
                email.Headers.Add("Return-Path", _options.Value.Email);
                email.Headers.Add("X-Mailer", "Microsoft Outlook 16.0"); // Looks familiar to spam filters

                var builder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = "This is a multi-part message in MIME format. Please view it in HTML capable client." // fallback
                };

                // Attach files if any
                if (attachments != null)
                {
                    foreach (var file in attachments)
                    {
                        if (file.Length > 0)
                        {
                            using var ms = new MemoryStream();
                            await file.CopyToAsync(ms);
                            builder.Attachments.Add(file.FileName, ms.ToArray(), ContentType.Parse(file.ContentType));
                        }
                    }
                }

                email.Body = builder.ToMessageBody();

                // Send the email
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_options.Value.Email, _options.Value.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                result.Succesd = true;
            }
            catch (Exception ex)
            {
                // Log the exception (you can improve this later to a logger)
                result.Succesd = false;
            }

            return result;
        }
    }
}