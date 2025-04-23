using Core.Meditor.User.Queries.Response;
using Core.Pagination;
using Domain.Enums.Oredring;
using MediatR;
using SchoolWep.Data.Enums.Oredring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Modles
{
    public class GetSellerPaginationModel:IRequest<PaginationResult<GetSellerPaginationReponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public OrederBy? OrederBy { get; set; }
        public SellerOredringEnum? sellerOredringEnum { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
    }
}
