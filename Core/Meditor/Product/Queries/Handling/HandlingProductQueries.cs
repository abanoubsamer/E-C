using AutoMapper;
using Core.Basic;
using Core.Meditor.Product.Queries.Models;
using Core.Meditor.Product.Queries.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using Domain;
using Domain.Dtos.Product.Queries;
using MediatR;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Queries.Handling
{
    public class HandlingProductQueries : ResponseHandler,
        IRequestHandler<GetProductListModelQueries, Response<List<GetProductListResponseQueries>>>,
        IRequestHandler<GetProductByIdModelQueries, Response<GetProductByIdResponsesQueries>>,
        IRequestHandler<GetProductPagination, PaginationResult<GetProducPaginationResponse>>,
        IRequestHandler<AutoCompleteSearchProductModel, Response<List<string>>>,
        IRequestHandler<GetProductMasterModel, Response<List<ProductMasterDto>>>
    {
        #region Filads

        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        #endregion Filads

        #region Constractor

        public HandlingProductQueries(IProductServices productServices, IMapper mapper)
        {
            _mapper = mapper;
            _productServices = productServices;
        }

        #endregion Constractor

        #region Handling

        public async Task<Response<List<GetProductListResponseQueries>>> Handle(GetProductListModelQueries request, CancellationToken cancellationToken)
        {
            var products = await _productServices.GetAllProductAsync();

            var prodmapping = _mapper.Map<List<GetProductListResponseQueries>>(products);

            return Success(prodmapping);
        }

        public async Task<Response<GetProductByIdResponsesQueries>> Handle(GetProductByIdModelQueries request, CancellationToken cancellationToken)
        {
            var products = await _productServices.GetProductByID(request.ProductId);

            if (products == null) return NotFound<GetProductByIdResponsesQueries>($"Not Found Product With Id: {request.ProductId}");

            var prodmapping = _mapper.Map<GetProductByIdResponsesQueries>(products);

            return Success(prodmapping);
        }

        public async Task<PaginationResult<GetProducPaginationResponse>> Handle(GetProductPagination request, CancellationToken cancellationToken)
        {
            var FilterQuery = _productServices.FilterStudent(request.ProductName, request.BrandId, request.ModelId, request.CategoryId, request.OrederBy, request.Oreder);
            var PaginationList = await FilterQuery.ToPaginationListAsync(request.PageNumber, request.PageSize);

            PaginationList.Meta = new
            {
                Count = PaginationList.Data.Count(),
                Date = DateTime.Now.ToShortDateString()
            };

            return PaginationList;
        }

        public async Task<Response<List<ProductMasterDto>>> Handle(GetProductMasterModel request, CancellationToken cancellationToken)
        {
            var prod = await _productServices.GetMasterProduct(request.SKU);
            if (prod == null) return NotFound<List<ProductMasterDto>>("Not Found");
            return Success(prod);
        }

        public async Task<Response<List<string>>> Handle(AutoCompleteSearchProductModel request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.SearchText)) return NotFound<List<string>>("Not Found");
            if (request.SearchText.Length < 3) return BadRequest<List<string>>("Search Text Must Be At Least 3 Characters");
            var products = await _productServices.SearchProductAsync(request.SearchText);
            return Success(products);
        }

        #endregion Handling
    }
}