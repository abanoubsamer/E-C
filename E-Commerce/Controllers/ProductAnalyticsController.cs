using Core.Meditor.ProductAnalytics.Commend.Models;
using Core.Meditor.ProductAnalytics.Quieres.Models;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace E_Commerce.Controllers
{

    [ApiController]
    public class ProductAnalyticsController : BasicController
    {
        public ProductAnalyticsController(IMediator mediator) : base(mediator)
        {
        }
        [Route(Routing.ProductAnalytics.addproductview)]
        [HttpPost]
        public async Task<IActionResult> AddProductView(AddProductViewModel Model)
        {
            return NewResult(await _Mediator.Send(Model));

        }
        [Route(Routing.ProductAnalytics.addproductinteraction)]
        [HttpPost]
        public async Task<IActionResult> AddProductInteraction(ProductInteractionModel Model)
        {
            return NewResult(await _Mediator.Send(Model));

        }
        [Route(Routing.ProductAnalytics.ProductBounceRates)]
        [HttpGet]
        public async Task<IActionResult> GetProductStats(string Id)
        {
            return NewResult(await _Mediator.Send(new GetProductStatsModel(Id)));

        }
        [Route(Routing.ProductAnalytics.SellerBounceRates)]
        [HttpGet]
        public async Task<IActionResult> GetSellerBounceRates(string Id)
        {
            return NewResult(await _Mediator.Send(new GetSellerOverViewAnalytics(Id)));

        }

    }
}
