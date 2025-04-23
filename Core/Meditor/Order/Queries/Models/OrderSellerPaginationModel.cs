using Core.Meditor.Order.Queries.Response;
using Core.Pagination;
using Domain.Dtos;
using Domain.Enums.Oredring;
using Domain.Enums.Status;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Queries.Models
{
    public class OrderSellerPaginationModel : IRequest<PaginationResult<GetSellerOrderDto>>
    {
        [Required]
        public required string SellerID { get; set; }
        public int PagaNamber { get; set; }
        public int PagaSize { get; set; }
        public string? SearchTearm { get; set; }  
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public OrderItemStatus? Status { get; set; }
        public OrederBy? orederBy { get; set; }
        public OrderOredringEnum? orderOredringEnum { get; set; }
        
    }
}
