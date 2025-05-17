using Couerses.Basic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.MetaData;
using Core.Meditor.Product.Commend.Models;
using Core.Meditor.Product.Queries.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Domain.Models;
using Services.ModelsServices;
using Infrastructure.Data.AppDbContext;
using Domain.Dtos;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class ProductController : BasicController
    {
        public ProductController(IMediator mediator) : base(mediator)
        {
        }

        //
        [HttpGet]
        [Route(Routing.Product.GetMaster)]
        public async Task<IActionResult> GetMaster(string Id)
        {
            return NewResult(await _Mediator.Send(new GetProductMasterModel(Id)));
        }

        [HttpPost]
        [Route(Routing.Product.Add)]
        public async Task<IActionResult> Add([FromForm] AddProductModelCommend entity)
        {
            return NewResult(await _Mediator.Send(entity));
        }

        [HttpPut]
        [Route(Routing.Product.Update)]
        public async Task<IActionResult> Update([FromForm] UpdateProductModelCommend entity)
        {
            return NewResult(await _Mediator.Send(entity));
        }

        [HttpGet]
        [Route(Routing.Product.List)]
        public async Task<IActionResult> List()
        {
            return NewResult(await _Mediator.Send(new GetProductListModelQueries()));
        }

        [HttpGet]
        [Route(Routing.Product.Pagination)]
        public async Task<IActionResult> ListPagination([FromQuery] GetProductPagination query)
        {
            return Ok(await _Mediator.Send(query));
        }

        [HttpGet]
        [Route(Routing.Product.GetById)]
        public async Task<IActionResult> GetById(string Id)
        {
            return NewResult(await _Mediator.Send(new GetProductByIdModelQueries(Id)));
        }

        [HttpDelete]
        [Route(Routing.Product.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            return NewResult(await _Mediator.Send(new DeleteProductModelQueries(Id)));
        }
    }
}