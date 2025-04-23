using Domain.Models;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NotificationServices
{
    public interface INotificationServices
    {
        public Task<ResultServices> SaveNotificationToken(UserFCMToken token);
        public Task SubscribeToTopic(string fcmToken, string topic);
        public Task SendNotificationToTopic(string topic, string title, string body);
        public Task SendUserNotificationAsync(string UserId , string title, string body);
        public  Task SendWeeklyNotification();

    }
}
