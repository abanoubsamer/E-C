using Core.Meditor.Seller.Commend.Models;
using Core.Meditor.Seller.Queries.Models;
using Core.Meditor.User.Queries.Modles;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{

    [ApiController]
    public class SellerController : BasicController
    {
        public SellerController(IMediator mediator) : base(mediator) { }

     
        [HttpGet]
        [Route(Routing.Seller.GetSellers)]
        public async Task<IActionResult> Sellers([FromQuery] GetSellerPaginationModel query)
        {
            return Ok(await _Mediator.Send(query));
        }

        [HttpGet]
        [Route(Routing.Seller.GetSellersById)]
        public async Task<IActionResult> GetSellersById(string Id)
        {
            return Ok(await _Mediator.Send(new GetSellerByIdModel(Id)));
        }

        [HttpPost]
        [Route(Routing.Seller.SellerEamilIsExist)]
        public async Task<IActionResult> SellerEamilIsExist(SellerEmialIsExistModel model)
        {
            return Ok(await _Mediator.Send(model));
        }


        [HttpGet]
        [Route(Routing.Seller.GetSellersProducts)]
        public async Task<IActionResult> GetSellerProduct([FromQuery] GetSellerProductsModels model)
        {
            return Ok(await _Mediator.Send(model));
        }

    }
}
