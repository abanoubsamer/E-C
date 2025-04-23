using Core.Basic;
using Core.Meditor.ProductAnalytics.Commend.Models;
using Domain.Models;
using MediatR;
using Services.ProductAnalyticsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;

namespace Core.Meditor.ProductAnalytics.Commend.Handler
{
    public class ProductAnalyticsHandler : ResponseHandler,
        IRequestHandler<AddProductViewModel, Response<string>>,
        IRequestHandler<ProductInteractionModel, Response<string>>
    {

        private readonly IProductAnalyticsServices _productAnalyticsServices;
        public ProductAnalyticsHandler(IProductAnalyticsServices productAnalyticsServices)
        {
            _productAnalyticsServices = productAnalyticsServices;
        }

        public async Task<Response<string>> Handle(AddProductViewModel request, CancellationToken cancellationToken)
        {
            var productView = new ProductView
            {
                ProductId = request.productId,
                UserId = request.userId,
                ViewDate = request.ViewDate 
            };

            var result = await _productAnalyticsServices.AddProductView(productView);

            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Success("Succed Add Product View");

        }

        public async Task<Response<string>> Handle(ProductInteractionModel request, CancellationToken cancellationToken)
        {
            var productInteraction = new ProductInteraction
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                InteractionType = request.InteractionType,
                InteractionDate = DateTime.UtcNow
            };

            var result = await _productAnalyticsServices.AddProductInteraction(productInteraction);

            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Success("Succed Add Product Interaction");

        }
    }
}
