using Domain.Enums.Notification;
using Domain.Models;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.NotificationServices
{
    public interface INotificationServices
    {
        public Task<ResultServices> SendRelaTimeNotificationAsync(string receiverId, string message, string title, NotificationReceiverType recevertype);

        public Task<List<Notification>> GetUserNotificationsAsync(string receiverId);

        public Task<List<Notification>> GetSellerNotificationsAsync(string receiverId);

        public Task<ResultServices> MarkNotificationAsReadAsync(string notificationId);

        public Task<ResultServices> AddNotificationAsync(Notification entity);

        public Task<ResultServices> UpdateNotificationAsync(Notification entity);

        public Task<ResultServices> DeleteNotificationAsync(string id);

        public Task<Notification> GetNotificationByIdAsync(string id);

        public Task<List<Notification>> FindMoreNotificationsAsync(Expression<Func<Notification, bool>> expression);

        public Task<Notification> FindOneNotificationsAsync(Expression<Func<Notification, bool>> expression);

        public Task<ResultServices> SaveNotificationToken(UserFCMToken token);

        public Task SubscribeToTopic(string fcmToken, string topic);

        public Task SendNotificationToTopic(string topic, string title, string body);

        public Task SendUserNotificationAsync(string UserId, string title, string body);

        public Task SendWeeklyNotification();

        public Task SendStockReminderNotificationAsync(string productId);
    }
}