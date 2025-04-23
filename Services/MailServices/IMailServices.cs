using Microsoft.AspNetCore.Http;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MailServices
{
    public interface IMailServices
    {
        public Task<ResultServices> SendMail(string MailTo, string Subject, string body, IList<IFormFile> Attechment = null);
    }
}
