using Core.ApiInterface;
using Core.Basic;
using Core.Meditor.Chat.Commend.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Refit;

namespace Core.Meditor.Chat.Commend.Handler
{
    public class ChatHandler : ResponseHandler, IRequestHandler<SendMassegeChatModel, Response<string>>
    {
        private readonly IOpenRouterApi _api;
        private readonly string _OpenAIApiKey;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(1);

        public ChatHandler(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _api = RestService.For<IOpenRouterApi>("https://openrouter.ai/api/v1");
            _OpenAIApiKey = configuration["OpenAI:ApiKey"] ?? "";
            _memoryCache = memoryCache;
        }

        public async Task<Response<string>> Handle(SendMassegeChatModel request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Text) && request.File == null)
            {
                return BadRequest<string>("يجب إدخال نص أو اختيار ملف على الأقل!");
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest<string>("يجب توفير معرف المستخدم (UserId).");
            }

            var chatHistory = _memoryCache.GetOrCreate(request.UserId, entry =>
            {
                entry.SlidingExpiration = _cacheExpiration; 
                return new List<Message>();
            });

            var contentArray = new List<ContentItem>();

            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                contentArray.Add(new ContentItem { type = "text", text = request.Text.Trim() });
            }

            if (request.File != null)
            {
                using var memoryStream = new MemoryStream();
                await request.File.CopyToAsync(memoryStream);
                string base64Image = Convert.ToBase64String(memoryStream.ToArray());

                contentArray.Add(new ContentItem
                {
                    type = "image_url",
                    image_url = new ImageUrl { url = $"data:image/png;base64,{base64Image}" }
                });
            }

            // إضافة الرسالة الجديدة إلى تاريخ المحادثة الخاص بالمستخدم
            chatHistory.Add(new Message { role = "user", content = contentArray });

            var requestData = new RequestData
            {
                model = "google/gemini-2.0-pro-exp-02-05:free",
                messages = chatHistory // إرسال المحادثة السابقة + الرسالة الجديدة
            };

            try
            {
                var response = await _api.SendMessageAsync(requestData, $"Bearer {_OpenAIApiKey}");

                var content = response.choices[0].message.content;

                // إضافة رد المساعد إلى المحادثة
                chatHistory.Add(new Message
                {
                    role = "assistant",
                    content = new List<ContentItem> { new ContentItem { type = "text", text = content.ToString() } }
                });

                // تحديث `MemoryCache` بالتاريخ الجديد
                _memoryCache.Set(request.UserId, chatHistory, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = _cacheExpiration // إعادة تعيين الوقت عند كل طلب
                });

                return Success(content.ToString());
            }
            catch (ApiException ex)
            {
                return BadRequest<string>(ex.Content);
            }
        }
    }
}
