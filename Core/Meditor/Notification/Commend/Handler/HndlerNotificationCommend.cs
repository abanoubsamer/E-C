using Core.Basic;
using Core.Meditor.Notification.Commend.Models;
using Domain.Models;
using MediatR;
using Services.ExtinsionServies;
using Services.NotificationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Notification.Commend.Handler
{
    public class HndlerNotificationCommend : ResponseHandler,
        IRequestHandler<SubscribeToTopicModelCommend, Response<string>>,
        IRequestHandler<SetTokenNotificationModelCommend, Response<string>>,
        IRequestHandler<SendNotificationUserRequest, Response<string>>,
        IRequestHandler<SendNotificationTopicRequest, Response<string>>
    {
        private readonly INotificationServices notificationServices;

        #region Failds

        #endregion

        #region Constractor
        public HndlerNotificationCommend(INotificationServices notificationServices)
        {
            this.notificationServices = notificationServices;
        }
        #endregion

        #region Hnalder
        public async Task<Response<string>> Handle(SubscribeToTopicModelCommend request, CancellationToken cancellationToken)
        {
           
            await notificationServices.SubscribeToTopic(request.token,request.topic);

            return Success("Succed Set Token");
        }

        public async Task<Response<string>> Handle(SendNotificationTopicRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await notificationServices.SendNotificationToTopic(request.Tobic, request.Title, request.Body);
                return Success("Succed Send Massage");
            }
            catch(Exception ex)
            {
                return BadRequest<string>(ex.InnerException.Message);
            }

        }

        public async Task<Response<string>> Handle(SetTokenNotificationModelCommend request, CancellationToken cancellationToken)
        {
            if (request.UserId.IsNullOrEmpty() || request.token.IsNullOrEmpty()) return BadRequest<string>("Invalid Request");

            var tokenmapping = new UserFCMToken
            {
                Token = request.token,
                UserId = request.UserId,
            };

             var result =  await  notificationServices.SaveNotificationToken(tokenmapping);

            if(!result.Succesd) return BadRequest<string>(result.Msg);

            return Success("Succed Set Token");

        }

        public async Task<Response<string>> Handle(SendNotificationUserRequest request, CancellationToken cancellationToken)
        {
            await notificationServices.SendUserNotificationAsync(request.UserId, request.Title, request.Body);
          
            return Success("Succed Send Notification");
        }
        #endregion


    }
}
