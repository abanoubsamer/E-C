using Domain.Enums.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Notification.Queires.Response
{
    public class GetNotificationsResponse
    {
        public string Id { get; set; }

        public NotificationReceiverType ReceiverType { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}