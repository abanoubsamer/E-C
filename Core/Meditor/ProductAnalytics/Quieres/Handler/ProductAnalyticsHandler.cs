using Core.Basic;
using Core.Meditor.ProductAnalytics.Quieres.Models;
using Core.Meditor.ProductAnalytics.Quieres.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.ProductAnalyticsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.ProductAnalytics.Quieres.Handler
{
    public class ProductAnalyticsHandler : ResponseHandler,
        IRequestHandler<GetProductStatsModel, Response<GetProductStatsResponse>>,
        IRequestHandler<GetSellerOverViewAnalytics, Response<GetSellerOverViewAnalyticsResponse >>
    {
        private readonly IProductAnalyticsServices _productAnalyticsServices;
        public ProductAnalyticsHandler(IProductAnalyticsServices productAnalyticsServices)
        {
            _productAnalyticsServices = productAnalyticsServices;
        }
        public async Task<Response<GetProductStatsResponse>> Handle(GetProductStatsModel request, CancellationToken cancellationToken)
        {
            var totalViews = await _productAnalyticsServices.GetProductViews(request.ProductId);
            var totalInteractions = await _productAnalyticsServices.GetProductInteractions(request.ProductId);
            
            double interactionRate = totalViews > 0 ? (double)totalInteractions / totalViews * 100 : 0;

            var result = new GetProductStatsResponse
            {
                interactionRate = interactionRate,
                TotalInteractions = totalInteractions,
                TotalViews = totalViews

            };
            return Success(result);
        }

      

        public async Task<Response<GetSellerOverViewAnalyticsResponse>> Handle(GetSellerOverViewAnalytics request, CancellationToken cancellationToken)
        {
            var totalViews = await _productAnalyticsServices.GetSellerViews(request.SellerId);
            var totalInteractions = await _productAnalyticsServices.GetSellerInteractions(request.SellerId);
            var totalComments = await _productAnalyticsServices.GetSellerComments(request.SellerId);
            var totalPostes = await _productAnalyticsServices.GetSellerPosts(request.SellerId);

            double interactionRate = totalViews > 0 ? (double)totalInteractions / totalViews * 100 : 0;

            var result = new GetSellerOverViewAnalyticsResponse
            {
                interactionRate = interactionRate,
                TotalInteractions = totalInteractions,
                TotalViews = totalViews,
                TotalComments = totalComments,
                TotalPosts = totalPostes

            };
            return Success(result);
        }
    }
}
