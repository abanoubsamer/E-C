using AutoMapper;
using Core.Basic;
using Core.Meditor.Reviews.Commend.Models;
using Domain.Models;
using MediatR;
using Services.ExtinsionServies;
using Services.ReviewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Commend.Handler
{
    public class ReviewHandlerCommend : ResponseHandler,
        IRequestHandler<AddReviewModelCommend, Response<string>>,
        IRequestHandler<UpdateReviewModelCommend, Response<string>>,
  
        IRequestHandler<DeleteReviewModelCommend, Response<string>>
    {

        #region Fildes
        private readonly IReviewServices _reviewServices;
        private readonly IMapper _Mapper ;
        #endregion

        #region Constractor
        public ReviewHandlerCommend(IReviewServices reviewServices,IMapper mapper)
        {
            _reviewServices = reviewServices;
            _Mapper = mapper;
        }

      
        #endregion

        #region Handler

        public async Task<Response<string>> Handle(AddReviewModelCommend request, CancellationToken cancellationToken)
        {
            var reviewMapping = _Mapper.Map<Review>(request);
            var result = await _reviewServices.AddReview(reviewMapping);
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Created("Succes Add Review");
        }

        public async Task<Response<string>> Handle(UpdateReviewModelCommend request, CancellationToken cancellationToken)
        {
            if (request.ReviewId.IsNullOrEmpty()) return BadRequest<string>("Id Is Requerd");

            var Review = await _reviewServices.GetReviewById(request.ReviewId);
          
            if (Review == null) return NotFound<string>("Not Found Is Review");

            
            Review = _Mapper.Map(request, Review);
            
            var result = await _reviewServices.UpdateReview(Review);
           
            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Updated<string>("Succesed Update Review");


        }

        public async Task<Response<string>> Handle(DeleteReviewModelCommend request, CancellationToken cancellationToken)
        {
            var result = await _reviewServices.DeleteReview(request.Id);
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Deleted<string>("Succes Delete Review");

        }

       


        #endregion

    }
}
