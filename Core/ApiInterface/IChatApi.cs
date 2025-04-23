using Microsoft.AspNetCore.Http;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApiInterface
{
    public interface IOpenRouterApi
    {
        [Post("/chat/completions")]
        Task<ApiResponse> SendMessageAsync([Body] RequestData requestData, [Header("Authorization")] string auth);
    }

    public class ImageUrl
    {
        public string url { get; set; }
    }

    public class ContentItem
    {
        public string type { get; set; }
        public string text { get; set; }
        public ImageUrl image_url { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public object content { get; set; } 
    }
    public class RequestData
    {
        public string model { get; set; }
        public bool stream { get; set; }
        public List<Message> messages { get; set; }

    }

    public class ApiResponse
    {
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
    }

   

}
