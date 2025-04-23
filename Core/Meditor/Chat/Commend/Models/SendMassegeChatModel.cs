using Core.Basic;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Chat.Commend.Models
{
    public class SendMassegeChatModel:IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public string Text { get; set; } // النص المرسل
        public IFormFile? File { get; set; } // الصورة المرسلة
    }
}
