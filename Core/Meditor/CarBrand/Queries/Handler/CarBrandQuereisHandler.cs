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
        IRequestHandler<GetCarBrandsModelWithPagtionation, PaginationResult<GetCarBrandResponse>>,
        IRequestHandler<GetCarBrandByIdModel, Response<GetCarBrandByIdResponse>>
    {
        private readonly ICarBrandServices carBrandServices;

        public CarBrandQuereisHandler(ICarBrandServices carBrandServices)
        {
            this.carBrandServices = carBrandServices;
        }

        public async Task<PaginationResult<GetCarBrandResponse>> Handle(GetCarBrandsModelWithPagtionation request, CancellationToken cancellationToken)
        {
            var expression = carBrandServices.Expression(x => new GetCarBrandResponse(x));
            var brands = await carBrandServices.GetCarBrandsPagedCachedAsync(request.PageNumber, request.PageSize);

            var paginationList = new PaginationResult<GetCarBrandResponse>
            {
                Data = brands.Select(expression.Compile()).ToList(),
                Meta = new
                {
                    Count = brands.Count,
                    Date = DateTime.Now.ToShortDateString()
                }
            };

            return paginationList;
        }

        public async Task<Response<GetCarBrandByIdResponse>> Handle(GetCarBrandByIdModel request, CancellationToken cancellationToken)
        {
            return await carBrandServices.GetCarBrandById(request.Id) is { } carBrand
                ? Success(new GetCarBrandByIdResponse(carBrand))
                : NotFound<GetCarBrandByIdResponse>($"Not Found Car Brand With Id: {request.Id}");
        }
    }
}