using Core.Basic;
using Core.Meditor.Notification.Queires.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Notification.Queires.Models
{
    public class GetUserNotificationsModel : IRequest<Response<List<GetNotificationsResponse>>>
    {
        public string UserId { get; set; }

        public GetUserNotificationsModel(string id)
        {
            UserId = id;
        }
    }
}