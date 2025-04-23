using Core.Meditor.Order.Queries.Response;
using Core.Pagination;
using Domain.Enums.Oredring;
using Domain.Enums.Status;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Queries.Models
{
    public class OrderPaginationModel:IRequest<PaginationResult<OrderPaginationResponse>>
    {
   
        public int PagaNamber { get; set; }
        public int PagaSize { get; set; }
        public string? OrderId { get; set; }
        public string?ProductID { get; set; }
        public string? UserID { get; set; }
        public DateTime? OrderDate { get; set; }
        public OrderStatus? Status { get; set; }
        public string? TransactionID { get; set; }
        public string? PaymentMethod { get; set; }
        public OrederBy? orederBy { get; set; }
        public OrderOredringEnum? orderOredringEnum { get; set; }
    }
}
