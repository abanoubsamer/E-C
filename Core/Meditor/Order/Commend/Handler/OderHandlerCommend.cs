using Core.Basic;
using Core.Dtos;
using Core.Meditor.Order.Commend.Models;
using Core.Meditor.Order.Commend.Response;
using Domain.Enums.Status;
using Domain.Models;
using MediatR;
using Newtonsoft.Json;
using Services.CardServices;
using Services.OderServices;
using Services.PaymentServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly ICardServices _cardServices;

        private readonly IPaymentServices _paymentServices;

        #endregion Filads

        #region Constractor

        public OderHandlerCommend(IOrderServices orderServices, ICardServices cardServices, IPaymentServices paymentServices)
        {
            _cardServices = cardServices;
            _paymentServices = paymentServices;
            _orderServices = orderServices;
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
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Updated<string>("Succesd Update Status Order");
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
            var order = new Domain.Models.Order
            {
                OrderID = Guid.NewGuid().ToString(),
                UserID = request.UserID,
                OrderDate = DateTime.Now,
                AddressId = request.shippingAddressId,
                PhoneId = request.phoneNumberId,
                TotalAmount = request.paymentMethod.Amount,
            };
            var orderItems = new List<Domain.Models.OrderItem>();

            foreach (var item in request.orderItems)
            {
                var price = await _orderServices.GetTotalAmountProduct(item.ProductID, item.Quantity);
                var sellerId = await _orderServices.GetsellerIdWithProductId(item.ProductID);

                orderItems.Add(new Domain.Models.OrderItem
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = price,
                    SellerID = sellerId,
                    Status = OrderItemStatus.Pending,
                });
            }

            order.OrderItems = orderItems;

            order.Payment = new Domain.Models.Payment
            {
                OrderID = order.OrderID,
                Amount = request.paymentMethod.Amount,
                PaymentMethod = request.paymentMethod.PaymentMethod,
                TransactionID = request.paymentMethod.TransactionID,
                PaymentDate = DateTime.Now
            };

            var result = await _orderServices.AddOrder(order);
            var resultDelete = await _cardServices.DeleteCardItemsUser(request.UserID);

            if (!result.Succesd || !resultDelete.Succesd) return BadRequest<string>(result.Msg);

            return Created<string>("Confime Payment Order");
        }

        #endregion Handler
    }
}