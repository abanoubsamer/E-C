
using Core.Pagination;
using Domain.Dtos.Seller.Quereis;
using MailKit.Search;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Queries.Models
{
    public class GetSellerProductsModels:IRequest<PaginationResult<GetSelleProductsResponseDto>>
    {
        public required string SellerID { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? searchTerm { get; set; }
    }
}
