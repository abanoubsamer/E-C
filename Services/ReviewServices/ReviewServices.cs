using Domain.Dtos.Review.Qureies;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SchoolWep.Data.Enums.Oredring;
using Services.ExtinsionServies;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.ReviewServices
{
    public class ReviewServices : IReviewServices
    {
        #region Fialds

        private readonly IUnitOfWork _unitOfWork;

        #endregion Fialds

        #region Constractor

        public ReviewServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constractor

        #region Implemntation

        public Expression<Func<Review, TResponse>> CreateExpression<TResponse>(Func<Review, TResponse> func)
        {
            return x => func(x);
        }

        public IQueryable<Review> FilterReview(string? ProductId, string? UserId, OrederBy? orederBy, ReviewOredringEnum? reviewOredringEnum)
        {
            //GetQueries
            var Query = GetQueryabl();

            //Filter
            if (!ProductId.IsNullOrEmpty())
            {
                Query = Query.Where(x => x.ProductID == ProductId);
            }
            if (!UserId.IsNullOrEmpty())
            {
                Query = Query.Where(x => x.UserID == UserId);
            }

            return Order(Query, orederBy, reviewOredringEnum);
        }

        public async Task<ResultServices> AddReview(Review entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Review" };
            try
            {
                await _unitOfWork.Repository<Review>().AddAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<ResultServices> DeleteReview(string id)
        {
            if (id.IsNullOrEmpty()) return new ResultServices { Msg = "Invalid Id" };
            try
            {
                var review = await _unitOfWork.Repository<Review>().FindOneAsync(x => x.ReviewID == id);
                if (review == null) return new ResultServices { Msg = "Not Found Review" };
                await _unitOfWork.Repository<Review>().DeleteAsync(review);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<ResultServices> UpdateReview(Review entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Review" };
            try
            {
                await _unitOfWork.Repository<Review>().UpdateAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public Task<Review> GetReviewById(string id)
        {
            return _unitOfWork.Repository<Review>().FindOneAsync(x => x.ReviewID == id);
        }

        private IQueryable<Review> Order(IQueryable<Review> Query, OrederBy? orederBy, ReviewOredringEnum? reviewOredringEnum)
        {
            // init
            bool Asinding = orederBy == null || orederBy == 0;
            reviewOredringEnum = reviewOredringEnum == null ? 0 : reviewOredringEnum;

            switch (reviewOredringEnum)
            {
                case ReviewOredringEnum.Date:
                    Query = Asinding ? Query.OrderBy(x => x.ReviewDate) : Query.OrderByDescending(x => x.ReviewDate);
                    break;

                case ReviewOredringEnum.Rating:
                    Query = Asinding ? Query.OrderBy(x => x.Rating) : Query.OrderByDescending(x => x.Rating);
                    break;
            }

            return Query;
        }

        private IQueryable<Review> GetQueryabl()
        {
            return _unitOfWork.Repository<Review>()
                .GetQueryable()
                .Include(x => x.Product)
                .Include(x => x.User);
        }

        public IQueryable<GetReviewPaginationResponseDto> FilterProductReviews(string ProductId, OrederBy? orederBy, ReviewOredringEnum? reviewOredringEnum)
        {
            var Queries = GetProductReviewsQueryabl(ProductId);

            return Order(Queries, orederBy, reviewOredringEnum);
        }

        private IQueryable<GetReviewPaginationResponseDto> Order(IQueryable<GetReviewPaginationResponseDto> Query, OrederBy? orederBy, ReviewOredringEnum? reviewOredringEnum)
        {
            // init
            bool Asinding = orederBy == null || orederBy == 0;
            reviewOredringEnum = reviewOredringEnum == null ? 0 : reviewOredringEnum;

            switch (reviewOredringEnum)
            {
                case ReviewOredringEnum.Date:
                    Query = Asinding ? Query.OrderBy(x => x.ReviewDate) : Query.OrderByDescending(x => x.ReviewDate);
                    break;

                case ReviewOredringEnum.Rating:
                    Query = Asinding ? Query.OrderBy(x => x.Rating) : Query.OrderByDescending(x => x.Rating);
                    break;
            }

            return Query;
        }

        private IQueryable<GetReviewPaginationResponseDto> GetProductReviewsQueryabl(string ProductId)
        {
            return _unitOfWork.Repository<Review>()
                .GetQueryable().Where(x => x.ProductID.Contains(ProductId)).Select(x => new GetReviewPaginationResponseDto
                {
                    Comment = x.Comment,
                    Rating = x.Rating,
                    ReviewDate = x.ReviewDate.ToString(),
                    ReviewID = x.ReviewID,
                    User = new Domain.Dtos.UserDto { Id = x.User.Id, Name = x.User.Name }
                });
        }

        public async Task<(Dictionary<int, double> Percentages, double AverageRating, int NumberReviews)> GetRatingStatistics(string productid)
        {
            var reviews = await _unitOfWork.Repository<Review>()
                .FindMoreAsNoTrackingAsync(x => x.ProductID.Contains(productid));

            if (!reviews.Any())
            {
                return (new Dictionary<int, double> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } }, 0, 0);
            }

            int totalReviews = reviews.Count();
            double averageRating = reviews.Average(r => r.Rating);

            var ratingCounts = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

            foreach (var review in reviews)
            {
                if (review.Rating >= 1 && review.Rating <= 5)
                {
                    ratingCounts[review.Rating]++;
                }
            }

            var ratingPercentages = ratingCounts.ToDictionary(
                kvp => kvp.Key,
                kvp => Math.Round((double)kvp.Value / totalReviews * 100, 2)
            );

            return (ratingPercentages, Math.Round(averageRating, 2), totalReviews);
        }

        #endregion Implemntation
    }
}