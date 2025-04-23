using Core.Basic;
using Core.Meditor.CarBrand.Queries.Response;
using Core.Pagination;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.CarBrand.Queries.Models
{
    public class GetCarBrandsModelWithPagtionation : IRequest<PaginationResult<GetCarBrandResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}