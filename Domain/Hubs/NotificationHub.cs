using Microsoft.AspNetCore.SignalR;

using System.Security.Claims;
using Domain.Hubs.Connection;

namespace Domain.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IConnectionManager _connectionManager;

        public NotificationHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                _connectionManager.AddConnection(Context.ConnectionId, userId);

                await Clients.Others.SendAsync("UserOnline", userId);
            }

            await base.OnConnectedAsync();
        }

        public async Task SendNotificationToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToSeller(string sellerId, string message)
        {
            await Clients.User(sellerId).SendAsync("ReceiveNotification", message);
        }
    }
}