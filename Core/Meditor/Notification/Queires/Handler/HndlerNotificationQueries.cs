using AutoMapper;
using Core.Basic;
using Core.Meditor.Notification.Queires.Models;
using Core.Meditor.Notification.Queires.Response;
using MediatR;
using Services.NotificationServices;
using static Domain.MetaData.Routing;

namespace Core.Meditor.Notification.Queires.Handler
{
    public class HndlerNotificationQueries : ResponseHandler
        , IRequestHandler<GetUserNotificationsModel, Response<List<GetNotificationsResponse>>>
        , IRequestHandler<GetSellerNotification, Response<List<GetNotificationsResponse>>>

    {
        private readonly INotificationServices notificationServices;
        private readonly IMapper _mapper;

        public HndlerNotificationQueries(INotificationServices notificationServices, IMapper mapper)
        {
            this._mapper = mapper;
            this.notificationServices = notificationServices;
        }

        public async Task<Response<List<GetNotificationsResponse>>> Handle(GetUserNotificationsModel request, CancellationToken cancellationToken)
        {
            var notifications = await notificationServices.GetUserNotificationsAsync(request.UserId);
            var MappedNotifications = _mapper.Map<List<GetNotificationsResponse>>(notifications);
            return Success(MappedNotifications);
        }

        public async Task<Response<List<GetNotificationsResponse>>> Handle(GetSellerNotification request, CancellationToken cancellationToken)
        {
            var notifications = await notificationServices.GetSellerNotificationsAsync(request.UserId);
            var MappedNotifications = _mapper.Map<List<GetNotificationsResponse>>(notifications);
            return Success(MappedNotifications);
        }
    }
}