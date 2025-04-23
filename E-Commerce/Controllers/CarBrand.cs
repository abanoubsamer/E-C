using Core.Meditor.CarBrand.Commend.Models;
using Core.Meditor.CarBrand.Queries.Models;
using Couerses.Basic;
using Domain.MetaData;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class CarBrand : BasicController
    {
        public CarBrand(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route(Routing.CarBrand.Pagination)]
        public async Task<IActionResult> Pagination([FromQuery] GetCarBrandsModelWithPagtionation model)
        {
            return Ok(await _Mediator.Send(model));
        }

        [HttpPost]
        [Route(Routing.CarBrand.Create)]
        public async Task<IActionResult> Create([FromForm] AddCarBrandModel model)
        {
            return NewResult(await _Mediator.Send(model));
        }
    }
}