using Core.Pagination;
using Domain.Dtos.Review.Qureies;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Queries.Models
{
    public class GetProductReviewsPaginationModel:IRequest<PaginationResult<GetReviewPaginationResponseDto>>    
    {
        public required string ProductId { get; set; }
        public int PageNamber { get; set; }
        public int PageSize { get; set; }
        public OrederBy orederBy { get; set; }
        public ReviewOredringEnum reviewOredringEnum { get; set; }
    }
}
