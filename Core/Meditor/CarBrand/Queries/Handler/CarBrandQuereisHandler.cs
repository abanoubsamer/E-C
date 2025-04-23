using Core.Basic;
using Core.Meditor.CarBrand.Queries.Models;
using Core.Meditor.CarBrand.Queries.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using MediatR;
using Services.CarBrandServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.CarBrand.Queries.Handler
{
    public class CarBrandQuereisHandler : ResponseHandler,
        IRequestHandler<GetCarBrandsModelWithPagtionation, PaginationResult<GetCarBrandResponse>>
    {
        private readonly ICarBrandServices carBrandServices;

        public CarBrandQuereisHandler(ICarBrandServices carBrandServices)
        {
            this.carBrandServices = carBrandServices;
        }

        public async Task<PaginationResult<GetCarBrandResponse>> Handle(GetCarBrandsModelWithPagtionation request, CancellationToken cancellationToken)
        {
            var expression = carBrandServices.Expression(x => new GetCarBrandResponse(x));
            var filter = carBrandServices.GetCarBrand();
            var paginationList = await filter.Select(expression).ToPaginationListAsync(request.PageNumber, request.PageSize);
            paginationList.Meta = new
            {
                Count = paginationList.Data.Count(),
                Date = DateTime.Now.ToShortDateString()
            };

            return paginationList;
        }
    }
}