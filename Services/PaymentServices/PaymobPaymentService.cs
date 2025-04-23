using Domain.Models;
using Services.PaymentServices;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class PaymobPaymentService : IPaymentServices
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2TVRBeU16UTJNU3dpYm1GdFpTSTZJbWx1YVhScFlXd2lmUS5HR3AyRzh4UF9CdlFlVnJtNENYU19TRlpoTS1laUVENVhHZFpXa0I3bjFudGtnaWJEc3B1SUI5NHhIcVo0VmxFY2FFb25PbGtWcjZpYm1YRktKSE9nZw==";

    public PaymobPaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

        public async Task<string> GetURLPaymentAsync(Order order,string authToken)
        {
            var url = "https://accept.paymob.com/api/auth/tokens";
            var jsonData = $"{{ \"api_key\": \"{_apiKey}\" }}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<AuthResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (jsonResponse == null || string.IsNullOrEmpty(jsonResponse.token))
            {
                throw new Exception("Failed to retrieve auth token.");
            }

      
        return await CreateOrder(jsonResponse.token, order, authToken);
        }

        private async Task<string> CreateOrder(string token,Order order,string authToken)
        {
             if (order == null || !order.OrderItems.Any())
                 throw new ArgumentException("Order is invalid or contains no items.");
                    var encryptedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(authToken)); // تشفير التوكن
                    var emailWithToken = $"token-{encryptedToken}@yourdomain.com";
              var data = new
              {
                auth_token = token,
                api_source = "INVOICE",
                amount_cents = (order.TotalAmount * 100).ToString(),
                currency = "EGP",
                integrations = new int[] { 4952415, 4952519, 4952520, 4952521 },
                 shipping_data = new
                 {
                     first_name = "Test",
                     last_name = "Account",
                     phone_number = "01010101010",
                     email = emailWithToken,
                 },
                items = order.OrderItems.Select(x => 
                new { 
                     name = x.Product.Name
                    ,amount_cents = ((x.Price * x.Quantity) * 100).ToString(),
                    quantity = x.Quantity.ToString(),
                    description = x.Product.Description,
                }).ToList(),
                delivery_needed = "false"
             };

            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://accept.paymob.com/api/ecommerce/orders", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }

             var responseBody = await response.Content.ReadAsStringAsync();
          
             var jsonResponse = JsonSerializer.Deserialize<PaymobResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

             return jsonResponse?.Url ?? "No URL received";
        }

        public class AuthResponse
        {
            public string token { get; set; }
        }

        public class PaymobResponse
        {
            public bool Succeeded { get; set; }
            public string Url { get; set; }
        }
}
