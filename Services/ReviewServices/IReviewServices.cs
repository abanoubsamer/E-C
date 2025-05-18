using Domain.Dtos.Review.Qureies;
using Domain.Models;
using SchoolWep.Data.Enums.Oredring;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ReviewServices
{
    public interface IReviewServices
    {
        public Expression<Func<Review, TResponse>> CreateExpression<TResponse>(Func<Review, TResponse> func);

        public IQueryable<Review> FilterReview(string? ProductId, string? UserId, OrederBy? orederBy, ReviewOredringEnum? reviewOredringEnum);

        public IQueryable<GetReviewPaginationResponseDto> FilterProductReviews(string ProductId, OrederBy? orederBy, ReviewOredringEnum? reviewOredringEnum);

        public Task<ResultServices> AddReview(Review entity);

        public Task<ResultServices> DeleteReview(string id);

        public Task<ResultServices> UpdateReview(Review entity);

        public Task<Review> GetReviewById(string id);

        public Task<(Dictionary<int, double> Percentages, double AverageRating, int NumberReviews)> GetRatingStatistics(string productid);
    }
}