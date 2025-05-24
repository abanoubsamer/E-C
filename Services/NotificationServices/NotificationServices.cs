using Domain.Models;
using Infrastructure.UnitOfWork;
using Services.Result;

using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using Domain.Hubs;
using Microsoft.EntityFrameworkCore;
using Domain.Enums.Notification;
using Domain.Hubs.Connection;
using static Domain.MetaData.Routing;

namespace Services.NotificationServices
{
    public class NotificationServices : INotificationServices
    {
        #region Failds

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<NotificationServices> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConnectionManager connectionManager;

        #endregion Failds

        #region Constractor

        public NotificationServices(IUnitOfWork unitOfWork,
            ILogger<NotificationServices> logger,
            IConnectionManager connectionManager,
            IHubContext<NotificationHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            _logger = logger;
            this.connectionManager = connectionManager;
            _hubContext = hubContext;
        }

        #endregion Constractor

        #region Implemntation

        #region FCM Notification

        public async Task<ResultServices> SaveNotificationToken(UserFCMToken token)
        {
            if (token == null) return new ResultServices { Msg = "Invalid Token" };

            var existingToken = await unitOfWork.Repository<UserFCMToken>().FindOneAsync(x => x.Token == token.Token && x.UserId == token.UserId);

            if (existingToken != null)
            {
                return new ResultServices { Succesd = true };
            }

            try
            {
                await unitOfWork.Repository<UserFCMToken>().AddAsync(token);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task SendNotificationAsync(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl = "https://drive.google.com/uc?export=view&id=1jk-WZNADhac60uEzgRuIauzm5SoJr_LK"
                }
                ,
                Data = new Dictionary<string, string>
                {
                    { "title", title },
                    { "body", body },
                    { "image", "https://drive.google.com/uc?export=view&id=1jk-WZNADhac60uEzgRuIauzm5SoJr_LK" }
                }
            };

            var messaging = FirebaseMessaging.DefaultInstance;

            try
            {
                var response = await messaging.SendAsync(message);
                Console.WriteLine("Notification sent: " + response);
            }
            catch (FirebaseMessagingException ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
                {
                    await RemoveInvalidToken(deviceToken);
                }
            }
        }

        public async Task SendUserNotificationAsync(string UserId, string title, string body)
        {
            var UserTokens = await unitOfWork.Repository<UserFCMToken>()
                .FindMoreAsNoTrackingAsync(x => x.UserId == UserId);

            await Parallel.ForEachAsync(UserTokens, async (token, _) =>
            {
                await SendNotificationAsync(token.Token, title, body);
            });
        }

        private async Task RemoveInvalidToken(string token)
        {
            var tokenEntry = await unitOfWork.Repository<UserFCMToken>().FindOneAsync(t => t.Token == token);

            if (tokenEntry != null)
            {
                await unitOfWork.Repository<UserFCMToken>().DeleteAsync(tokenEntry);
            }
        }

        public async Task SubscribeToTopic(string fcmToken, string topic)
        {
            try
            {
                await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new[] { fcmToken }, topic);

                Console.WriteLine($"Device subscribed to topic: {topic}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error subscribing to topic: {ex.Message}");
            }
        }

        public async Task UnsubscribeFromTopic(string fcmToken, string topic)
        {
            try
            {
                // الغاء الاشتراك من الموضوع
                await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(new[] { fcmToken }, topic);

                Console.WriteLine($"Device unsubscribed from topic: {topic}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unsubscribing from topic: {ex.Message}");
            }
        }

        public async Task SendNotificationToTopic(string topic, string title, string body)
        {
            var message = new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body,
                },
                Topic = topic,
            };

            try
            {
                // إرسال الرسالة إلى الموضوع
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
        }

        public async Task SendWeeklyNotification()
        {
            _logger.LogInformation("Sending weekly notification...");

            await SendNotificationToTopic("Seller", "Weekly Update", "Here's your weekly update!");

            _logger.LogInformation("Weekly notification sent successfully!");
        }

        #endregion FCM Notification

        #region UserNotification

        public async Task<ResultServices> AddNotificationAsync(Domain.Models.Notification entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Notification" };
            try
            {
                await unitOfWork.Repository<Domain.Models.Notification>().AddAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices
                {
                    Msg = ex.Message
                };
            }
        }

        public async Task<ResultServices> UpdateNotificationAsync(Domain.Models.Notification entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Notification" };
            try
            {
                await unitOfWork.Repository<Domain.Models.Notification>().UpdateAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices
                {
                    Msg = ex.Message
                };
            }
        }

        public async Task<ResultServices> DeleteNotificationAsync(string id)
        {
            var notification = unitOfWork.Repository<Domain.Models.Notification>().FindOneAsync(x => x.Id == id);
            if (notification == null) return new ResultServices { Msg = "Notification not found" };
            try
            {
                await unitOfWork.Repository<Domain.Models.Notification>().DeleteAsync(notification.Result);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<Domain.Models.Notification> GetNotificationByIdAsync(string id)
        {
            return await unitOfWork.Repository<Domain.Models.Notification>().FindOneAsync(x => x.Id == id);
        }

        public async Task<List<Domain.Models.Notification>> FindMoreNotificationsAsync(Expression<Func<Domain.Models.Notification, bool>> expression)
        {
            return await unitOfWork.Repository<Domain.Models.Notification>().FindMoreAsNoTrackingAsync(expression);
        }

        public async Task<Domain.Models.Notification> FindOneNotificationsAsync(Expression<Func<Domain.Models.Notification, bool>> expression)
        {
            return await unitOfWork.Repository<Domain.Models.Notification>().FindOneWithNoTrackingAsync(expression);
        }

        public async Task<ResultServices> SendRelaTimeNotificationAsync(string receiverId, string message, string title, NotificationReceiverType recevertype)
        {
            try
            {
                var notification = new Domain.Models.Notification
                {
                    ReceiverId = receiverId,
                    Message = message,
                    Title = title,
                    IsRead = false,
                    ReceiverType = recevertype,

                    CreatedAt = DateTime.UtcNow
                };

                var connectionid = connectionManager.GetUserConnections(receiverId);

                await _hubContext.Clients.Clients(connectionid).SendAsync("ReceiveNotification", notification);

                await unitOfWork.Repository<Domain.Models.Notification>().AddAsync(notification);

                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices
                {
                    Succesd = false,
                    Msg = $"Error sending notification: {ex.Message}"
                };
            }
        }

        public async Task<List<Domain.Models.Notification>> GetUserNotificationsAsync(string receiverId)
        {
            return await unitOfWork.Repository<Domain.Models.Notification>().GetQueryable()
                .Where(x => x.ReceiverId == receiverId).OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ResultServices> MarkNotificationAsReadAsync(string notificationId)
        {
            var notification = await unitOfWork.Repository<Domain.Models.Notification>()
                .FindOneAsync(x => x.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await unitOfWork.SaveChangesAsync();
                return new ResultServices { Succesd = true };
            }
            return new ResultServices { Msg = "Notification not found" };
        }

        public async Task<List<Domain.Models.Notification>> GetSellerNotificationsAsync(string receiverId)
        {
            return await unitOfWork.Repository<Domain.Models.Notification>()
                 .FindMoreAsNoTrackingAsync(x => x.ReceiverId == receiverId
                  && x.ReceiverType == NotificationReceiverType.Seller);
        }

        public async Task SendStockReminderNotificationAsync(string productId)
        {
            var product = await unitOfWork.Repository<ProductListing>().FindOneAsync(p => p.ProductID == productId);
            if (product == null) return;

            string message = $"Reminder: Please update the stock for your product '{product.Name}'.";

            await SendUserNotificationAsync(product.Seller.UserID, "Update Stock Reminder", message);
            await SendRelaTimeNotificationAsync(product.Seller.UserID, message, "Stock Reminder"
                , NotificationReceiverType.Seller);
        }

        #endregion UserNotification

        #endregion Implemntation
    }
}