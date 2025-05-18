using Core.Meditor.Reviews.Queries.Response;
using Core.Pagination;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Queries.Models
{
    public class GetReviewsPaginationModel : IRequest<PaginationResult<GetReviewPaginationResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? ProductId { get; set; }
        public string? UserId { get; set; }
        public OrederBy orederBy { get; set; }
        public ReviewOredringEnum reviewOredringEnum { get; set; }
    }
}