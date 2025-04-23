using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Notification.Commend.Models
{
    public class SubscribeToTopicModelCommend : IRequest<Response<string>>
    {
        public string token { get; set; }
        public string topic { get; set; }
 
    }
}
