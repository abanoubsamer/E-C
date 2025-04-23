using Core.Meditor.Category.Commend.Models;
using Core.Meditor.Category.Querires.Models;
using Core.Meditor.Product.Queries.Models;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class CategoryController : BasicController
    {
        public CategoryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route(Routing.Category.Pagination)]
        public async Task<IActionResult> ListPagination([FromQuery] GetCategoryPagination query)
        {
            return Ok(await _Mediator.Send(query));
        }

        [HttpGet]
        [Route(Routing.Category.GetById)]
        public async Task<IActionResult> GetById(string Id)
        {
            return Ok(await _Mediator.Send(new GetCtegoryByIdModelQueries(Id)));
        }

        [HttpGet]
        [Route(Routing.Category.GetParent)]
        public async Task<IActionResult> GetParentCategories()
        {
            return Ok(await _Mediator.Send(new GetParentCategoriesModel()));
        }

        [HttpGet]
        [Route(Routing.Category.GetSub)]
        public async Task<IActionResult> GetSubcategories(string Id)
        {
            return Ok(await _Mediator.Send(new GetSubcategoriesModel(Id)));
        }

        [HttpPost]
        [Route(Routing.Category.Add)]
        public async Task<IActionResult> Add([FromForm] AddCategoryModelCommend Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }

        [HttpDelete]
        [Route(Routing.Category.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            return NewResult(await _Mediator.Send(new DeleteCtegoryModelQueries(Id)));
        }

        [HttpPut]
        [Route(Routing.Category.Update)]
        public async Task<IActionResult> Update(UpdateCategoryModelCommend Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }
    }
}