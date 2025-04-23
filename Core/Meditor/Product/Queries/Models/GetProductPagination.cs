using Core.Meditor.Product.Queries.Response;
using Core.Pagination;
using Domain.Dtos.Product.Queries;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Queries.Models
{
    public class GetProductPagination : IRequest<PaginationResult<GetProducPaginationResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public ProductOredringEnum? Oreder { get; set; }
        public OrederBy? OrederBy { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryId { get; set; }
        public string? ModelId { get; set; }
        public string? BrandId { get; set; }
    }
}