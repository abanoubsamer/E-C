using AutoMapper;
using Core.Basic;
using Core.Meditor.Seller.Queries.Models;
using Core.Meditor.Seller.Queries.Response;
using Core.Meditor.User.Queries.Modles;
using Core.Meditor.User.Queries.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using Domain.Dtos.Seller.Quereis;
using MediatR;
using Services.SellerServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Queries.Handler
{
    public class SellerHandlerQueries : ResponseHandler,
        IRequestHandler<GetSellerPaginationModel, PaginationResult<GetSellerPaginationReponse>>,
        IRequestHandler<GetSellerByIdModel, Response<GetSellerByIdResponse>>,
        IRequestHandler<GetsellerProductByIdModel, Response<GetSellerProductByIdResponse>>,
        IRequestHandler<GetSellerProductsModels, PaginationResult<GetSelleProductsResponseDto>>
    {
        #region Fialds

        private readonly ISellerServices _sellerServices;
        private readonly IMapper _Mapper;

        #endregion Fialds

        #region Constractor

        public SellerHandlerQueries(ISellerServices sellerServices, IMapper Mapper)
        {
            _Mapper = Mapper;
            _sellerServices = sellerServices;
        }

        #endregion Constractor

        #region Handler

        public async Task<PaginationResult<GetSellerPaginationReponse>> Handle(GetSellerPaginationModel request, CancellationToken cancellationToken)
        {
            //create Expretion
            var Expression = _sellerServices.CreateExpression(x => new GetSellerPaginationReponse(x));
            // filter
            var Filtet = _sellerServices.FilterSeller(request.UserName, request.Email, request.City, request.Country, request.PostalCode, request.State, request.Street, request.OrederBy, request.sellerOredringEnum);

            var PaginationList = await Filtet.Select(Expression).ToPaginationListAsync(request.PageNumber, request.PageSize);

            PaginationList.Meta = new
            {
                Date = DateTime.Now.ToString(),
                Total = PaginationList.Data.Count()
            };

            return PaginationList;
        }

        public async Task<PaginationResult<GetSelleProductsResponseDto>> Handle(GetSellerProductsModels request, CancellationToken cancellationToken)
        {
            // filter
            var Filtet = _sellerServices.FilterSellerProduct(request.SellerID, request.searchTerm);

            var PaginationList = await Filtet.ToPaginationListAsync(request.PageNumber, request.PageSize);

            PaginationList.Meta = new
            {
                Date = DateTime.Now.ToString(),
                Total = PaginationList.Data.Count()
            };

            return PaginationList;
        }

        public async Task<Response<GetSellerByIdResponse>> Handle(GetSellerByIdModel request, CancellationToken cancellationToken)
        {
            var seller = await _sellerServices.GetSellerById(request.Id);
            if (seller == null) return NotFound<GetSellerByIdResponse>("Not Found Seller");
            var sellerMapping = _Mapper.Map<GetSellerByIdResponse>(seller);
            return Success(sellerMapping);
        }

        public async Task<Response<GetSellerProductByIdResponse>> Handle(GetsellerProductByIdModel request, CancellationToken cancellationToken)
        {
            var product = await _sellerServices.GetSellerProductById(request.Id);
            if (product == null) return NotFound<GetSellerProductByIdResponse>("Not Found Product");
            var productMapping = _Mapper.Map<GetSellerProductByIdResponse>(product);
            return Success(productMapping);
        }

        #endregion Handler
    }
}