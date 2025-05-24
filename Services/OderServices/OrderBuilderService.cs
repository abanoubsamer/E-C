using Domain.Enums.Status;
using Domain.Models;
using Services.ProductServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OderServices
{
    public class OrderBuilderService : IOrderBuilderService
    {
        private readonly IOrderServices _orderServices;
        private readonly IProductServices _productServices;

        public OrderBuilderService(IProductServices productServices, IOrderServices orderServices)
        {
            _productServices = productServices;
            _orderServices = orderServices;
        }

        public async Task<(Domain.Models.Order order, ResultServices result)> BuildOrderAsync(Order request)
        {
            var order = new Domain.Models.Order
            {
                OrderID = Guid.NewGuid().ToString(),
                UserID = request.UserID,
                OrderDate = DateTime.Now,
                AddressId = request.AddressId,
                PhoneId = request.PhoneId,
                TotalAmount = request.TotalAmount,
            };

            var orderItems = new List<Domain.Models.OrderItem>();

            foreach (var item in request.OrderItems)
            {
                var isAvailable = await _productServices.CheckStockAsync(item.ProductID, item.Quantity);
                if (!isAvailable)
                    return (null, new ResultServices { Msg = "Product is not available" });

                var price = await _orderServices.GetTotalAmountProduct(item.ProductID, item.Quantity);
                var seller = await _orderServices.GetsellerIdWithProductId(item.ProductID);

                orderItems.Add(new Domain.Models.OrderItem
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = price,
                    SellerID = seller.SellerID,
                    Seller = seller,
                    Status = OrderItemStatus.Pending
                });
            }

            order.OrderItems = orderItems;

            order.Payment = new Domain.Models.Payment
            {
                OrderID = order.OrderID,
                Amount = request.Payment.Amount,
                PaymentMethod = request.Payment.PaymentMethod,
                TransactionID = request.Payment.TransactionID,
                PaymentDate = DateTime.Now
            };

            return (order, new ResultServices { Succesd = true });
        }
    }
}