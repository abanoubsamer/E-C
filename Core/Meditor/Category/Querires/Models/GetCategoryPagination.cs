using Core.Basic;
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
    public class GetCategorys : IRequest<Response<List<GetCategoryResponseQueries>>>
    {
    }
}