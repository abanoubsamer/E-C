using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Notification.Commend.Models
{
    public class SendNotificationUserRequest : IRequest<Response<string>>
    {

        public string UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
