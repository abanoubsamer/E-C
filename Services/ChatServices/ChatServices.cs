using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Services.Result;

namespace Services.ChatServices
{
    public class ChatServices : IChatServices
    {
        private const string ApiUrl = "https://openrouter.ai/api/v1/chat/completions";

        private readonly string _OpenAIApiKey;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(1);
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatServices(ILogger<ChatServices> logger, IConfiguration configuration, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _OpenAIApiKey = configuration["OpenAI:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public class SendMassegeChatModel
        {
            public string UserId { get; set; }
            public string Text { get; set; }
            public IFormFile? File { get; set; }
        }

        public async IAsyncEnumerable<string> SendMassage(SendMassegeChatModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Text) && request.File == null)
                yield break;

            if (string.IsNullOrEmpty(request.UserId))
                yield break;

            // جلب سجل المحادثة من الكاش
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

            // إضافة الرسالة الجديدة إلى السجل
            chatHistory.Add(new Message { role = "user", content = contentArray });

            // الاحتفاظ بآخر 10 رسائل فقط
            chatHistory = chatHistory.TakeLast(10).ToList();

            // تحضير الطلب بحيث يتم فهم المحادثة لكن يتم الرد فقط على آخر رسالة
            var requestData = new RequestData
            {
                model = "google/gemini-2.0-flash-exp:free",
                messages = new List<Message>
                {
                    new Message { role = "system", content = "This is the chat history for context, but only respond to the last message." }
                },
                stream = true
            };

            // إضافة الرسائل السابقة كمعلومات سياقية فقط بدون الرد عليها
            foreach (var msg in chatHistory.Take(chatHistory.Count - 1))
            {
                requestData.messages.Add(new Message { role = msg.role, content = msg.content });
            }

            // إرسال آخر رسالة فقط للرد عليها
            requestData.messages.Add(new Message { role = "user", content = contentArray });

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _OpenAIApiKey);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
            {
                Content = JsonContent.Create(requestData)
            };

            using var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
            {
                yield return "An error occurred while communicating with the AI.";
                yield break;
            }

            await foreach (var line in ReadStream(response.Content))
            {
                if (line.StartsWith("data:"))
                {
                    var jsonString = line["data:".Length..].Trim();

                    if (jsonString.Equals("[DONE]", StringComparison.OrdinalIgnoreCase))
                    {
                        yield break;
                    }

                    var jsonData = JsonNode.Parse(jsonString);

                    if (jsonData?["choices"] is JsonArray choicesArray && choicesArray.Count > 0)
                    {
                        var messageContent = choicesArray[0]?["delta"]?["content"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(messageContent))
                        {
                            yield return messageContent;
                        }
                    }
                }
            }
        }

        private async IAsyncEnumerable<string> ReadStream(HttpContent content)
        {
            await using var stream = await content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    yield return line;
                }
            }
        }

        public async Task<ResultServices> NewChat(string userId)
        {
            _memoryCache.Remove(userId);
            return new ResultServices
            {
                Succesd = true,
                Msg = "New chat started."
            };
        }
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
}