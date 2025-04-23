using Domain.OptionsConfiguration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<ResultServices> SendMail(string MailTo, string Subject, string body, IList<IFormFile> Attechment = null)
        {
            //sender
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Value.Email),
                Subject = Subject,
            };
            //resever
            email.To.Add(MailboxAddress.Parse(MailTo));

            var builder = new BodyBuilder();

            if (Attechment != null)
            {
                byte[] filesbyte;
                foreach (var file in Attechment)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        filesbyte = ms.ToArray();
                        builder.Attachments.Add(file.FileName, filesbyte, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));

            //provider 
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(_options.Value.Email, _options.Value.Password);

            await smtp.SendAsync(email);

             smtp.Disconnect(true);

            return new ResultServices { Succesd = true };
        }
    }
}
