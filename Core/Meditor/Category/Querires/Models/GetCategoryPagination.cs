using Core.Meditor.Category.Querires.Response;
using Core.Meditor.Product.Queries.Response;
using Core.Pagination;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Querires.Models
{
    public class GetCategoryPagination : IRequest<PaginationResult<GetCategoryPaginagtionResponseQueries>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public OrederBy? OrederBy { get; set; }
        public string? CategoryName { get; set; }

    }
}
