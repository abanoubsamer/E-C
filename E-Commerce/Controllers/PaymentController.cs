using Core.Meditor.Order.Commend.Models;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
 
    [ApiController]
    public class PaymentController : BasicController
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
          
        }

        [HttpPost]
        [Route(Routing.Payment.GetUrlpayment)]
        public async Task<IActionResult> GetUrlpayment(JWTOrderModelCommend Model)
        {
            return NewResult(await _Mediator.Send(Model));

        }
    }
}
