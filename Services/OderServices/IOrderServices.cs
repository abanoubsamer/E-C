using Domain.Dtos;
using Domain.Enums.Oredring;
using Domain.Enums.Status;
using Domain.Models;
using SchoolWep.Data.Enums.Oredring;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.OderServices
{
    public interface IOrderServices
    {
        public Task<OrderWithTokenResult> GetOrderWithJWT(Domain.Models.Order entity);

        public Task<decimal> GetTotalAmountProduct(string ProductId, int Quantity);

        public Task<Seller> GetsellerIdWithProductId(string ProductId);

        public Task<ResultUpdateStatusOrder> UpdateStatusOrder(string OrderId, string productId, OrderItemStatus Status);

        public Task<ResultServices> AddOrder(Domain.Models.Order entity);

        public Task<ResultServices> DeleteOrderFormDb(string Id);

        public Task<ResultServices> CancelOrder(string Id);

        public IQueryable<Domain.Models.Order> FilterOrder(
          string? OrderId,
          string? ProductID,
          string? UserID,
          DateTime? OrderDate,
          OrderStatus? Status,
          string? TransactionID,
          string? PaymentMethod,
          OrederBy? orederBy,
          OrderOredringEnum? orderOredringEnum);

        public Expression<Func<T, TResponse>> expression<T, TResponse>(Func<T, TResponse> func);

        public Task<Domain.Models.Order> GetOrderById(string id);

        public Task<List<Domain.Models.Order>> GetUserOrders(string Userid);

        public IQueryable<GetSellerOrderDto> GetSellerOrders(string Sellerid,
          string? Searchtearm,

          DateTime? fromDate,
          DateTime? toDate,
          OrderItemStatus? Status,
          OrederBy? orederBy,
          OrderOredringEnum? orderOredringEnum);

        public Task CancelOrderIfAllPendingAsync(string orderId);
    }
}