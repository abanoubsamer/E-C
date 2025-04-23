using Core.Meditor.Mail.Commend.Models;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{

    [ApiController]
    public class MailController : BasicController
    {
        public MailController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route(Routing.Mail.SendOtp)]
        public async Task<IActionResult> SendEmailOtp(SendEmailOtp model)
        {
            return NewResult(await _Mediator.Send(model));
        }
        [HttpPost]
        [Route(Routing.Mail.VerifyOtp)]
        public async Task<IActionResult> VerifyOtp(VerifyOtpModel model)
        {
            return NewResult(await _Mediator.Send(model));
        }


    }
}
