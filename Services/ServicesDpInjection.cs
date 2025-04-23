using Microsoft.Extensions.DependencyInjection;
using Services.AuthenticationServices;
using Services.CarBrandServices;
using Services.CardServices;
using Services.CategoryServices;
using Services.ChatServices;
using Services.FileSystemServices;
using Services.MailServices;
using Services.ModelCompatibilityServices;

using Services.ModelsServices;
using Services.NotificationServices;
using Services.OderServices;
using Services.PaymentServices;
using Services.ProductAnalyticsServices;
using Services.ProductServices;
using Services.ReviewServices;
using Services.SellerServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ServicesDpInjection
    {
        public static IServiceCollection AddServiesInjection(this IServiceCollection services)
        {
            services.AddTransient<IFileServices, FileServices>();
            services.AddTransient<IUserServices, UserServices.UserServices>();
            services.AddTransient<IReviewServices, ReviewServices.ReviewServices>();
            services.AddTransient<IProductServices, ProductServices.ProductServices>();
            services.AddTransient<ICategoryServices, CategoryServices.CategoryServices>();
            services.AddTransient<IAuthenticationServices, AuthenticationServices.AuthenticationServices>();
            services.AddTransient<ISellerServices, SellerServices.SellerServices>();
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddTransient<IPaymentServices, PaymobPaymentService>();
            services.AddTransient<ICardServices, CardServices.CardServices>();
            services.AddTransient<INotificationServices, NotificationServices.NotificationServices>();
            services.AddTransient<IMailServices, MailServices.MailServices>();
            services.AddTransient<IProductAnalyticsServices, ProductAnalyticsServices.ProductAnalyticsServices>();
            services.AddTransient<IChatServices, ChatServices.ChatServices>();
            services.AddTransient<IModelsServices, ModelsServices.ModelsServices>();

            services.AddTransient<IModelCompatibilityServices, ModelCompatibilityServices.ModelCompatibilityServices>();
            services.AddTransient<ICarBrandServices, CarBrandServices.CarBrandServices>();

            return services;
        }
    }
}