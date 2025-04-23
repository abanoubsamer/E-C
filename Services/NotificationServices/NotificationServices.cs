using Domain.Models;
using Infrastructure.UnitOfWork;
using Services.Result;

using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;


namespace Services.NotificationServices
{
    public class NotificationServices : INotificationServices
    {
         #region Failds
            private readonly IUnitOfWork unitOfWork;
            private readonly ILogger<NotificationServices> _logger;
        #endregion

        #region Constractor
        public NotificationServices(IUnitOfWork unitOfWork, ILogger<NotificationServices> logger)
        {
            this.unitOfWork = unitOfWork;
            _logger = logger;
        }
            #endregion

            #region Implemntation
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
            catch (Exception ex) {

                return new ResultServices { Msg = ex.Message };
            }
        }

            public async Task SendNotificationAsync(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
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
                    Notification = new Notification
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
        #endregion
    }
}
