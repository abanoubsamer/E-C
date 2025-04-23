using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.ChatServices;
using System.IO.Pipelines;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using static Services.ChatServices.ChatServices;

namespace E_Commerce.Controllers
{
    [ApiController]

    public class ChatController : ControllerBase
    {

        private readonly IChatServices _chatService;
        public ChatController(IChatServices chatService)
        {
           _chatService = chatService;
        }

        public record ChatRequest(string Message);

        [HttpPost]
        [Route(Routing.Chat.SendMassage)]
        public async Task<IActionResult> Chat([FromForm] SendMassegeChatModel request)
        {
            Response.ContentType = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Connection"] = "keep-alive";

            await foreach (var message in _chatService.SendMassage(request))
            {
                await Response.WriteAsync($"{message}");
                await Response.Body.FlushAsync();
            }

            return new EmptyResult();
        }


      
    }
}
