using Core.Meditor.Card.Commend.Models;
using Core.Meditor.Card.Queries.Models;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    public class CardController : BasicController
    {
        public CardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route(Routing.Card.AddCardItems)]
        public async Task<IActionResult> AddCardItems(AddCardItemModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }

        [HttpDelete]
        [Route(Routing.Card.Delete)]
        public async Task<IActionResult> DeleteCardItems(string Id)
        {
            return NewResult(await _Mediator.Send(new DeleteCardItemModelCommend(Id)));
        }

        [HttpPut]
        [Route(Routing.Card.Update)]
        public async Task<IActionResult> UpdateCardItems(UpdateUserCardItemsModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }

        [HttpGet]
        [Route(Routing.Card.GetUserCard)]
        public async Task<IActionResult> GetUserCard(string Id)
        {
            return NewResult(await _Mediator.Send(new GetCardUserModelQueries(Id)));
        }
    }
}