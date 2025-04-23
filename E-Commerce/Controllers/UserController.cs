using Core.Meditor.Product.Commend.Models;
using Core.Meditor.Product.Queries.Models;
using Core.Meditor.User.Commend.Model;
using Core.Meditor.User.Commend.Models;
using Core.Meditor.User.Queries.Model;
using Core.Meditor.User.Queries.Modles;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{

    [ApiController]
    public class UserController : BasicController
    {

        public UserController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [Route(Routing.User.GetUsers)]
        public async Task<IActionResult> Users([FromQuery] GetUserPaginationModel query)
        {
            return Ok(await _Mediator.Send(query));
        }

        [HttpGet]
        [Route(Routing.User.GetUserById)]
        public async Task<IActionResult> UserById(string Id)
        {
            return NewResult(await _Mediator.Send(new GetUserByIdModel(Id)));

        }

        [HttpGet]
        [Route(Routing.User.GetShippingAddresses)]
        public async Task<IActionResult> GetShippingAddresses(string Id)
        {
            return NewResult(await _Mediator.Send(new GetSippingAddressUserModels(Id)));

        }

        [HttpPost]
        [Route(Routing.User.AddShippingAddresses)]
        public async Task<IActionResult> AddUserShippingAddresses(AddUserShippingAddressesModel Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }

        [HttpPost]
        [Route(Routing.User.AddPhones)]
        public async Task<IActionResult> AddUserPhones(AddUserPhonesModel Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }

        [HttpPut]
        [Route(Routing.User.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserModel Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }


    }
}
