using Core.Meditor.Reviews.Commend.Models;
using Core.Meditor.Reviews.Queries.Models;
using Core.Meditor.User.Queries.Modles;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{

    [ApiController]
    public class ReviewController : BasicController
    {

        public ReviewController(IMediator mediator) :base(mediator) { }
        [HttpGet]
        [Route(Routing.Review.Pagination)]
        public async Task<IActionResult> Reviews([FromQuery] GetReviewsPaginationModel query)
        {
            return Ok(await _Mediator.Send(query));
        }
       
        
        [HttpGet]
        [Route(Routing.Review.GetRatingStatistics)]
        public async Task<IActionResult> GetRatingStatistics(string Id)
        {
        
            return Ok(await _Mediator.Send(new GetRatingStatisticsModels(Id)));
        }

        [HttpGet]
        [Route(Routing.Review.ProductReview)]
        public async Task<IActionResult> ProductReview([FromQuery] GetProductReviewsPaginationModel query)
        {
            return Ok(await _Mediator.Send(query));
        }

        [HttpPost]
        [Route(Routing.Review.Add)]
        public async Task<IActionResult> Add(AddReviewModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }
        [HttpPut]
        [Route(Routing.Review.Update)]
        public async Task<IActionResult> Update(UpdateReviewModelCommend model)
        {
            return NewResult(await _Mediator.Send(model));
        }
        [HttpDelete]
        [Route(Routing.Review.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            return NewResult(await _Mediator.Send(new DeleteReviewModelCommend(Id)));
        }



    }
}
