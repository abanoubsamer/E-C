using Core.Meditor.Order.Commend.Models;
using Core.Meditor.Order.Queries.Models;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.PaymentServices;
using Services.Result;
using System.Net;
using System.Net.Http;
using System.Text;

using System.Text.Json;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class OrderController : BasicController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger) : base(mediator)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route(Routing.Order.Pagination)]
        public async Task<IActionResult> Pagination([FromQuery] OrderPaginationModel Query)
        {
            return Ok(await _Mediator.Send(Query));
        }

        [HttpGet]
        //[Authorize]
        [Route(Routing.Order.GetUserOrders)]
        public async Task<IActionResult> GetUserOrders(string Id)
        {
            return NewResult(await _Mediator.Send(new GetUserOrdersModel(Id)));
        }

        [HttpGet]
        [Route(Routing.Order.GetSellerOrders)]
        public async Task<IActionResult> GetSellerOrders([FromQuery] OrderSellerPaginationModel Query)
        {
            return Ok(await _Mediator.Send(Query));
        }

        [HttpDelete]
        [Route(Routing.Order.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            return NewResult(await _Mediator.Send(new DeleteOrderModelCommend(Id)));
        }

        [HttpPut]
        [Route(Routing.Order.Cancel)]
        public async Task<IActionResult> Cancel(string Id)
        {
            return NewResult(await _Mediator.Send(new CancelOrdermodelCommend(Id)));
        }

        [HttpPut]
        [Route(Routing.Order.UpdateStatus)]
        public async Task<IActionResult> UpdateStatus(UpdateStatusOrderModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }

        [HttpPost]
        [Route(Routing.Order.ConfirmOrderPaymentWithAdd)]
        public async Task<IActionResult> stats([FromBody] JsonElement data)
        {
            var json = data.GetRawText(); // تحويل JsonElement إلى نص JSON

            var res = JsonSerializer.Deserialize<ConfimPaymentOrderModelCommend>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return NewResult(await _Mediator.Send(res));
        }

        [HttpPost]
        [Route(Routing.Order.AddTest)]
        public async Task<IActionResult> Add(AddOrderTestModel model)
        {
            return NewResult(await _Mediator.Send(model));
        }
    }
}