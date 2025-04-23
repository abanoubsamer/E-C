using AutoMapper;
using Core.Basic;
using Core.Meditor.Order.Queries.Models;
using Core.Meditor.Order.Queries.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Services.ExtinsionServies;
using Services.OderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Queries.Handler
{
    public class OrderHandlerQueries : ResponseHandler,
        IRequestHandler<OrderPaginationModel, PaginationResult<OrderPaginationResponse>>,
        IRequestHandler<OrderSellerPaginationModel, PaginationResult<GetSellerOrderDto>>,
        IRequestHandler<GetUserOrdersModel, Response<List<GetUserOrdersResponse>>>
    {


        #region Filads
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor
        public OrderHandlerQueries(IOrderServices orderServices, IMapper mapper)
        {
            _mapper = mapper;
            _orderServices = orderServices; 
        }
        #endregion

        #region Handler
        public async Task<PaginationResult<OrderPaginationResponse>> Handle(OrderPaginationModel request, CancellationToken cancellationToken)
        {
            //create Expression
            var Expression = _orderServices.expression<Domain.Models.Order, OrderPaginationResponse>(x => new OrderPaginationResponse(x));
            // Filter 
            var Filter = _orderServices.FilterOrder(request.OrderId, request.ProductID, request.UserID, request.OrderDate, request.Status, request.TransactionID, request.PaymentMethod, request.orederBy, request.orderOredringEnum);
            // Pagination List 
            var PaginationList = await Filter.Select(Expression).ToPaginationListAsync(request.PagaNamber, request.PagaSize);

            PaginationList.Meta = new
            {
                Count = PaginationList.Data.Count(),
            };

            return PaginationList;
        }

        public async Task<Response<List<GetUserOrdersResponse>>> Handle(GetUserOrdersModel request, CancellationToken cancellationToken)
        {
            var Order = await _orderServices.GetUserOrders(request.Id);

            if (Order == null) return NotFound<List<GetUserOrdersResponse>>("Not Found Orders");

            var OrderMapping = _mapper.Map<List<GetUserOrdersResponse>>(Order);
             
            return Success(OrderMapping);

        }

        public async Task<PaginationResult<GetSellerOrderDto>> Handle(OrderSellerPaginationModel request, CancellationToken cancellationToken)
        {
      
            var Filter = _orderServices.GetSellerOrders(request.SellerID, request.SearchTearm, request.fromDate,request.toDate, request.Status, request.orederBy, request.orderOredringEnum);
            // Pagination List 
            var PaginationList = await Filter.ToPaginationListAsync(request.PagaNamber, request.PagaSize);

            PaginationList.Meta = new
            {
                Count = PaginationList.Data.Count(),
            };

            return PaginationList;
        }
        #endregion


    }
}
