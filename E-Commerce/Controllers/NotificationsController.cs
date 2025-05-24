using Core.Meditor.Notification.Commend.Models;
using Core.Meditor.Notification.Queires.Models;
using Couerses.Basic;
using Domain.MetaData;
using Domain.Models;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.NotificationServices;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class NotificationsController : BasicController
    {
        public NotificationsController(IMediator mediator) : base(mediator)
        {
        }

        //GetNotifications

        [HttpGet]
        [Route(Routing.Notification.GetUserNotification)]
        public async Task<IActionResult> GetNotificationsUser(string Id)
        {
            return NewResult(await _Mediator.Send(new GetUserNotificationsModel(Id)));
        }

        [HttpGet]
        [Route(Routing.Notification.GetSellerNotification)]
        public async Task<IActionResult> GetSellerNotification(string Id)
        {
            return NewResult(await _Mediator.Send(new GetSellerNotification(Id)));
        }

        [HttpPost]
        [Route(Routing.Notification.SendNotificationTopic)]
        public async Task<IActionResult> SendNotificationToTopic(SendNotificationTopicRequest request)
        {
            return NewResult(await _Mediator.Send(request));
        }

        [HttpPost]
        [Route(Routing.Notification.SetNotificationTokenTopic)]
        public async Task<IActionResult> SubscribeToTopic(SubscribeToTopicModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }

        [HttpPost]
        [Route(Routing.Notification.SendNotificationToUser)]
        public async Task<IActionResult> SendNotificationToUser(SendNotificationUserRequest request)
        {
            return NewResult(await _Mediator.Send(request));
        }

        [HttpPost]
        [Route(Routing.Notification.SetTokenNotificationToUser)]
        public async Task<IActionResult> SetTokenNotificationToUser(SetTokenNotificationModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }
    }
}