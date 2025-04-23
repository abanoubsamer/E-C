using Core.Basic;
using Core.Meditor.Reviews.Queries.Models;
using Core.Meditor.Reviews.Queries.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using Domain.Dtos.Review.Qureies;
using MediatR;
using Services.ReviewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Queries.Handler
{
    public class ReviewHandlerQueries : ResponseHandler,
        IRequestHandler<GetReviewsPaginationModel, PaginationResult<GetReviewPaginationResponse>>,
        IRequestHandler<GetRatingStatisticsModels, Response<GetRatingStatisticsResponse>>,
        IRequestHandler<GetProductReviewsPaginationModel, PaginationResult<GetReviewPaginationResponseDto>>
    {

        #region Fialds
        private readonly IReviewServices _reviewServices;
        #endregion

        #region Constractor
        public ReviewHandlerQueries(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }
        #endregion


        #region Handler

        #endregion

        public async Task<PaginationResult<GetReviewPaginationResponse>> Handle(GetReviewsPaginationModel request, CancellationToken cancellationToken)
        {
            //create Expression
            var Exp = _reviewServices.CreateExpression(x => new GetReviewPaginationResponse(x));
            // Filter
            var Filter = _reviewServices.FilterReview(request.ProductId,request.UserId,request.orederBy,request.reviewOredringEnum);
            //PAGINATION
            var PaginationList = await Filter.Select(Exp).ToPaginationListAsync(request.PageNamber, request.PageSize);

            PaginationList.Meta = new
            {
                Date = DateTime.Now.ToString(),
                Count = PaginationList.Data.Count()
            };

            return PaginationList;
        }

        public async Task<PaginationResult<GetReviewPaginationResponseDto>> Handle(GetProductReviewsPaginationModel request, CancellationToken cancellationToken)
        {
            var Filter = _reviewServices.FilterProductReviews(request.ProductId, request.orederBy, request.reviewOredringEnum);
            //PAGINATION
            var PaginationList = await Filter.ToPaginationListAsync(request.PageNamber, request.PageSize);

            PaginationList.Meta = new
            {
                Date = DateTime.Now.ToString(),
                Count = PaginationList.Data.Count()
            };

            return PaginationList;
        }

        public async Task<Response<GetRatingStatisticsResponse>> Handle(GetRatingStatisticsModels request, CancellationToken cancellationToken)
        {
            var (percentages, averageRating,numberReviews) = await _reviewServices.GetRatingStatistics(request.productid);

            var result = new GetRatingStatisticsResponse
            {
                AverageRating = averageRating,
                NamberReviews   = numberReviews,
                Percentages = percentages,
            };
            return Success(result);
        }
    }
}
