using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Notification
{
    public partial class NotificationMapping
    {
        private void GetUserNotifications()
        {
            CreateMap<Domain.Models.Notification, Core.Meditor.Notification.Queires.Response.GetNotificationsResponse>();
        }
    }
}