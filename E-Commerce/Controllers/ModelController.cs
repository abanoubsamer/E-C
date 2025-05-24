using Core.Meditor.Model.Commend.Models;
using Core.Meditor.Model.Queires.Models;
using Couerses.Basic;
using Domain.MetaData;
using Domain.Models;
using Infrastructure.Data.AppDbContext;
using Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class ModelController : BasicController
    {
        public ModelController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route(Routing.Model.GetModelById)]
        public async Task<IActionResult> GetModelById(string Id)
        {
            return NewResult(await _Mediator.Send(new GetModelById(Id)));
        }

        [HttpGet]
        [Route(Routing.Model.GetModelWithBrand)]
        public async Task<IActionResult> AddModelCompatibility(string Id)
        {
            return NewResult(await _Mediator.Send(new GetModels(Id)));
        }

        [HttpPost]
        [Route(Routing.Model.AddModelCompatibility)]
        public async Task<IActionResult> AddModelCompatibility(AddModelCompatibilityModel command)
        {
            return NewResult(await _Mediator.Send(command));
        }

        [HttpPost]
        [Route(Routing.Model.AddModel)]
        public async Task<IActionResult> AddModel([FromForm] AddModel command)
        {
            return NewResult(await _Mediator.Send(command));
        }
    }
}