using Core.Basic;
using MediatR;


namespace Core.Meditor.Notification.Commend.Models
{
    public class SendNotificationTopicRequest : IRequest<Response<string>>
    {
        public string Tobic { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
