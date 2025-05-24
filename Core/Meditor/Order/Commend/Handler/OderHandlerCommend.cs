using Core.Basic;
using Core.Dtos;
using Core.Meditor.Order.Commend.Models;
using Core.Meditor.Order.Commend.Response;
using Domain.Dtos.Product.Commend;
using Domain.Enums.Notification;
using Domain.Enums.Status;
using Domain.Models;
using Hangfire;
using MediatR;
using Newtonsoft.Json;
using SchoolWep.Data.Enums.Oredring;
using Services.CardServices;
using Services.NotificationServices;
using Services.OderServices;
using Services.PaymentServices;
using Services.ProductServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;

namespace Core.Meditor.Order.Commend.Handler
{
    public class OderHandlerCommend : ResponseHandler,
        IRequestHandler<JWTOrderModelCommend, Response<string>>,
        IRequestHandler<ConfimPaymentOrderModelCommend, Response<string>>,
        IRequestHandler<DeleteOrderModelCommend, Response<string>>,
        IRequestHandler<UpdateStatusOrderModelCommend, Response<string>>,
        IRequestHandler<CancelOrdermodelCommend, Response<string>>,
        IRequestHandler<AddOrderTestModel, Response<string>>
    {
        #region Filads

        private readonly IOrderServices _orderServices;
        private readonly IProductServices productServices;
        private readonly ICardServices _cardServices;
        private readonly IOrderBuilderService _orderBuilderService;
        private readonly IPaymentServices _paymentServices;
        private readonly INotificationServices _notificationService;

        #endregion Filads

        #region Constractor

        public OderHandlerCommend(IOrderServices orderServices,
            IOrderBuilderService orderBuilderService,
            IProductServices productServices,
            ICardServices cardServices,
            IPaymentServices paymentServices,
            INotificationServices notificationServices)
        {
            _orderBuilderService = orderBuilderService;
            _cardServices = cardServices;
            _notificationService = notificationServices;
            _paymentServices = paymentServices;
            _orderServices = orderServices;
            this.productServices = productServices;
        }

        #endregion Constractor

        #region Handler

        public async Task<Response<string>> Handle(JWTOrderModelCommend request, CancellationToken cancellationToken)
        {
            var OrderMapp = new Domain.Models.Order
            {
                OrderID = Guid.NewGuid().ToString(),
                UserID = request.UserID,
                PhoneId = request.phoneId,
                AddressId = request.shippingAddressID,
            };

            OrderMapp.OrderItems = request.orderItems.Select(x => new OrderItem
            {
                OrderID = OrderMapp.OrderID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
            }).ToList();

            var resutl = await _orderServices.GetOrderWithJWT(OrderMapp);

            if (!resutl.IsAuthenticated) return BadRequest<string>(resutl.Messgage);

            // var url = await _paymentServices.GetURLPaymentAsync(resutl.Order,resutl.Token);

            return Created(resutl.Token);
        }

        public async Task<Response<string>> Handle(DeleteOrderModelCommend request, CancellationToken cancellationToken)
        {
            var result = await _orderServices.DeleteOrderFormDb(request.Id);
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Deleted<string>("Succed Delete Order");
        }

        public async Task<Response<string>> Handle(CancelOrdermodelCommend request, CancellationToken cancellationToken)
        {
            var result = await _orderServices.CancelOrder(request.Id);
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Deleted<string>("Succed Cancel Order");
        }

        public async Task<Response<string>> Handle(UpdateStatusOrderModelCommend request, CancellationToken cancellationToken)
        {
            var result = await _orderServices.UpdateStatusOrder(request.OrderId, request.ProductID, request.Status);

            if (!result.Succesd)
                return BadRequest<string>(result.Msg);

            var orderItem = result.OrderItem;
            if (orderItem == null || orderItem.Seller == null || orderItem.Seller.User == null)
            {
                return BadRequest<string>("Invalid order item or seller information.");
            }

            var notificationResult = new ResultServices();
            string message;

            switch (orderItem.Status)
            {
                case OrderItemStatus.Confirm:
                    message = $"Order #{orderItem.OrderID} has been confirmed by seller {orderItem.Seller.User.Name}.";
                    notificationResult = await _notificationService.SendRelaTimeNotificationAsync(orderItem.Order.UserID, message, "Confirm Order", NotificationReceiverType.User);
                    break;

                case OrderItemStatus.Shipped:
                    message = $"Order #{orderItem.OrderID} has been shipped by seller {orderItem.Seller.User.Name}.";
                    notificationResult = await _notificationService.SendRelaTimeNotificationAsync(orderItem.Order.UserID, message, "Shipped Order", NotificationReceiverType.User);
                    break;

                case OrderItemStatus.Delivered:
                    message = $"Order #{orderItem.OrderID} has been delivered by seller {orderItem.Seller.User.Name}.";
                    notificationResult = await _notificationService.SendRelaTimeNotificationAsync(orderItem.Order.UserID, message, "Delivered Order", NotificationReceiverType.User);
                    break;

                case OrderItemStatus.Cancelled:
                    message = $"Order #{orderItem.OrderID} has been cancelled by seller {orderItem.Seller.User.Name}.\n" +
                        $" Because {request.cancellationReason}.";
                    notificationResult = await _notificationService.SendRelaTimeNotificationAsync(orderItem.Order.UserID, message, "Cancelled Order", NotificationReceiverType.User);
                    break;

                default:
                    message = $"Order #{orderItem.OrderID} status updated to {orderItem.Status} by seller {orderItem.Seller.User.Name}.";
                    notificationResult = await _notificationService.SendRelaTimeNotificationAsync(orderItem.Order.UserID, message, "Order Update", NotificationReceiverType.User);
                    break;
            }

            await _notificationService.SendUserNotificationAsync(orderItem.Order.UserID, "Order Update", message);

            if (!notificationResult.Succesd)
            {
                return BadRequest<string>(notificationResult.Msg);
            }

            return Updated<string>("Order status updated successfully and notification sent.");
        }

        public async Task<Response<string>> Handle(ConfimPaymentOrderModelCommend request, CancellationToken cancellationToken)
        {
            if (request.obj.success)
            {
                var email = request.obj.order.shipping_data.email;

                if (email.StartsWith("token-"))
                {
                    var base64Token = email.Split('@')[0].Replace("token-", "");
                    var token = Encoding.UTF8.GetString(Convert.FromBase64String(base64Token));
                    var jwthandeler = new JwtSecurityTokenHandler();
                    var AccessToken = jwthandeler.ReadToken(token) as JwtSecurityToken;
                    var claims = AccessToken?.Claims;

                    var ClaimOrder = claims.FirstOrDefault(x => x.Type == "OrderData");
                    var orderData = JsonConvert.DeserializeObject<Domain.Models.Order>(ClaimOrder.Value);
                    var order = new Domain.Models.Order
                    {
                        OrderID = Guid.NewGuid().ToString(),
                        UserID = orderData.UserID,
                        OrderDate = orderData.OrderDate,
                        AddressId = orderData.AddressId,
                        PhoneId = orderData.PhoneId,
                        TotalAmount = orderData.TotalAmount,
                    };
                    order.OrderItems = orderData.OrderItems.Select(item => new Domain.Models.OrderItem
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        SellerID = item.SellerID,
                        Status = item.Status,
                    }).ToList();
                    order.Payment = new Domain.Models.Payment
                    {
                        OrderID = order.OrderID,
                        Amount = (request.obj.amount_cents / 100),
                        PaymentMethod = request.obj.source_data.type + ":" + request.obj.source_data.sub_type,
                        TransactionID = request.obj.id.ToString(),
                        PaymentDate = request.obj.order.created_at
                    };

                    var result = await _orderServices.AddOrder(order);
                    if (!result.Succesd) return BadRequest<string>(result.Msg);

                    return Created<string>("Confime Payment Order");
                }
                return BadRequest<string>("Invalid Token");
            }
            return BadRequest<string>("Faild Procces Payment");
        }

        public async Task<Response<string>> Handle(AddOrderTestModel request, CancellationToken cancellationToken)
        {
            try
            {
                var Mapp = new Domain.Models.Order
                {
                    AddressId = request.shippingAddressId,
                    PhoneId = request.phoneNumberId,
                    UserID = request.UserID,
                    Payment = new Domain.Models.Payment
                    {
                        UserID = request.UserID,
                        TransactionID = request.paymentMethod.TransactionID,

                        PaymentMethod = request.paymentMethod.PaymentMethod,
                        Amount = request.paymentMethod.Amount
                    },
                    OrderItems = request.orderItems.Select(x => new Domain.Models.OrderItem
                    {
                        ProductID = x.ProductID,
                        Quantity = x.Quantity,
                    }).ToList()
                };

                var (order, result) = await _orderBuilderService.BuildOrderAsync(Mapp);
                if (!result.Succesd) return BadRequest<string>(result.Msg);

                var resultOrder = await _orderServices.AddOrder(order);
                if (!resultOrder.Succesd) return BadRequest<string>(resultOrder.Msg);

                var resultDelete = await _cardServices.DeleteCardItemsUser(request.UserID);
                if (!resultDelete.Succesd) return BadRequest<string>(resultDelete.Msg);

                foreach (var item in order.OrderItems)
                {
                    var updateResult = await productServices.UpdateStockAsync(item.ProductID, item.Quantity);
                    if (!updateResult.Succesd) return BadRequest<string>(updateResult.Msg);
                }

                var sellerIds = order.OrderItems.Select(x => x.Seller.UserID).Distinct();
                string msg = $"New Order #{order.OrderID} has been placed by {order.UserID} and waiting for confirmation.";

                foreach (var sellerId in sellerIds)
                {
                    await _notificationService.SendRelaTimeNotificationAsync(sellerId, msg, "New Order", NotificationReceiverType.Seller);
                    await _notificationService.SendUserNotificationAsync(sellerId, "New Order", msg);
                }

                BackgroundJob.Schedule(() =>
                                _orderServices.CancelOrderIfAllPendingAsync(order.OrderID),
                                TimeSpan.FromHours(24));

                return Created<string>("Confirm Payment Order");
            }
            catch (Exception ex)
            {
                return BadRequest<string>("An unexpected error occurred.");
            }
        }

        #endregion Handler
    }
}