using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MetaData
{
    public static class Routing
    {
        public const string SingelId = "{Id}";
        public const string SingelName = "{Name}";
        public const string EmailName = "{Email}";
        public const string Text = "{Text}";
        public const string Root = "Api";
        public const string Version = "V1";
        public const string Role = Root + "/" + Version + "/";

        public static class Product
        {
            public const string Prefix = Role + "Product/";
            public const string List = Prefix + "List";
            public const string GetMaster = Prefix + "GetMaster/" + SingelId;
            public const string AutoCompleteSearch = Prefix + "AutoCompleteSearch/" + Text;
            public const string Add = Prefix + "Add";
            public const string Update = Prefix + "Update";
            public const string Pagination = Prefix + "Pagination";
            public const string Delete = Prefix + SingelId;

            public const string GetById = Prefix + SingelId;
        }

        public static class CarBrand
        {
            public const string Prefix = Role + "CarBrand/";
            public const string Create = Prefix + "Create";
            public const string GetById = Prefix + SingelId;
            public const string Pagination = Prefix + "Pagination";
        }

        public static class Model
        {
            public const string Prefix = Role + "Model/";
            public const string AddModelCompatibility = Prefix + "AddModelCompatibility";
            public const string AddModel = Prefix + "AddModel";
            public const string GetModelWithBrand = Prefix + "GetModelWithBrand/" + SingelId;
            public const string GetModelById = Prefix + "GetModelById/" + SingelId;
        }

        public static class Mail
        {
            public const string Prefix = Role + "Mail/";
            public const string SendOtp = Prefix + "SendOtp";
            public const string VerifyOtp = Prefix + "VerifyOtp";
        }

        public static class Chat
        {
            public const string Prefix = Role + "Chat/";
            public const string SendMassage = Prefix + "SendMassage";
            public const string NewChat = Prefix + "NewChat/" + SingelId;
        }

        public static class ProductAnalytics
        {
            public const string Prefix = Role + "ProductAnalytics/";
            public const string addproductview = Prefix + "add-product-view";
            public const string addproductinteraction = Prefix + "add-product-interaction";
            public const string ProductBounceRates = Prefix + "product-bounce-rates/" + SingelId;
            public const string SellerBounceRates = Prefix + "Seller-bounce-rates/" + SingelId;
        }

        public static class Category
        {
            public const string Prefix = Role + "Category/";
            public const string Add = Prefix + "Add";
            public const string Update = Prefix + "Update";
            public const string All = Prefix + "All";
            public const string Delete = Prefix + SingelId;
            public const string GetById = Prefix + SingelId;
            public const string GetParent = Prefix + "parents";
            public const string GetSub = Prefix + "subcategories/" + SingelId;
        }

        public static class Review
        {
            public const string Prefix = Role + "Review/";
            public const string Add = Prefix + "Add";
            public const string Update = Prefix + "Update";
            public const string Pagination = Prefix + "Pagination";
            public const string ProductReview = Prefix + "ProductReview";
            public const string GetRatingStatistics = Prefix + "GetRatingStatistics/" + SingelId;
            public const string Delete = Prefix + SingelId;
            public const string GetById = Prefix + SingelId;
        }

        public static class Order
        {
            public const string Prefix = Role + "Order/";
            public const string ConfirmOrderPaymentWithAdd = Prefix + "ConfirmOrderPaymentWithAdd";

            //   public const string Add = Prefix + "Add";
            public const string Update = Prefix + "Update";

            public const string AddTest = Prefix + "AddTest";

            public const string UpdateStatus = Prefix + "UpdateStatus";
            public const string Pagination = Prefix + "Pagination";
            public const string Delete = Prefix + SingelId;
            public const string Cancel = Prefix + "Cancel/" + SingelId;
            public const string GetById = Prefix + SingelId;
            public const string GetUserOrders = Prefix + "UserOrders/" + SingelId;
            public const string GetSellerOrders = Prefix + "SellerOrders/";
        }

        public static class Card
        {
            public const string Prefix = Role + "Card/";
            public const string GetUserCard = Prefix + SingelId;
            public const string Pagination = Prefix + "Pagination";

            //user
            public const string SubPrefix = Role + "UserCard/";

            public const string AddCardItems = SubPrefix + "Add";
            public const string Update = SubPrefix + "Update";
            public const string Delete = SubPrefix + SingelId;
        }

        public static class User
        {
            public const string Prefix = Role + "User/";
            public const string GetUsers = Prefix + "GetUsers";
            public const string UpdateUser = Prefix + "UpdateUser";
            public const string AddShippingAddresses = Prefix + "AddShippingAddresses";
            public const string GetShippingAddresses = Prefix + "ShippingAddresses/" + SingelId;
            public const string AddPhones = Prefix + "AddPhones";
            public const string GetPhones = Prefix + "GetPhones/" + SingelId;
            public const string GetUserById = Prefix + SingelId;
        }

        public static class Seller
        {
            public const string Prefix = Role + "Seller/";
            public const string GetSellers = Prefix + "GetSellers";
            public const string GetSellersById = Prefix + SingelId;
            public const string GetSellersProducts = Prefix + "GetSellersProducts";
            public const string GetSellerProductById = Prefix + "GetSellerProductById/" + SingelId;
            public const string SellerEamilIsExist = Prefix + "SellerEamilIsExist";
        }

        public static class Payment
        {
            public const string Prefix = Role + "Payment/";
            public const string GetUrlpayment = Prefix + "GetUrlpayment";
        }

        public static class Notification
        {
            public const string Prefix = Role + "Notification/";
            public const string SetNotificationTokenTopic = Prefix + "SetNotificationTokenTopic";
            public const string GetUserNotification = Prefix + "GetUserNotification/" + SingelId;
            public const string GetSellerNotification = Prefix + "GetSellerNotification/" + SingelId;
            public const string SetTokenNotificationToUser = Prefix + "SetTokenNotificationToUser";
            public const string SendNotificationTopic = Prefix + "SendNotificationTopic";
            public const string SendNotificationToUser = Prefix + "SendNotificationToUser";
        }

        public static class Authentication
        {
            public const string Prefix = Role + "Auth/";
            public const string RegisterUser = Prefix + "Register/User";
            public const string RegisterSeller = Prefix + "Register/Seller";
            public const string Login = Prefix + "Login";
            public const string LoginSeller = Prefix + "LoginSeller";
            public const string LoginWihtGoogle = Prefix + "Google-Login";
            public const string AuthCallBackGoogle = Prefix + "Google-Response";
            public const string EmailExist = Prefix + "EmailExist/" + EmailName;
            public const string UserNameExist = Prefix + "UserNameExist/" + SingelName;
            public const string RefreshToken = Prefix + "RefreshToken/";
            public const string ValidationToken = Prefix + "ValidationToken/";
            public const string GetToken = Prefix + "Get-Token/";
            public const string GetRefreshToken = Prefix + "Get-RefreshToken/";
        }
    }
}