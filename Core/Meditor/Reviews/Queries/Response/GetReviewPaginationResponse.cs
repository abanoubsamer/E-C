using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos;
using Domain.Dtos;

namespace Core.Meditor.Reviews.Queries.Response
{
    public class GetReviewPaginationResponse
    {
        public string ReviewID { get; set; }

        public UserDto User { get; set; }

        public ProductDto Product { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public string ReviewDate { get; set; }

        public GetReviewPaginationResponse(Review review)
        {
            ReviewID = review.ReviewID;
            User = new UserDto { Id = review.User.Id, Email = review.User.Email, Name = review.User.Name };
            Product = new ProductDto { Id = review.Product.ProductID, Name = review.Product.Name };
            Rating = review.Rating;
            Comment = review.Comment;
            ReviewDate = review.ReviewDate.ToLongDateString();
        }
    }
}