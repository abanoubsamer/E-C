using Core.Basic;
using Core.Meditor.Seller.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Queries.Models
{
    public class GetSellerByIdModel : IRequest<Response<GetSellerByIdResponse>>
    {

        public string Id { get; set; }
        public GetSellerByIdModel(string id)
        {
            Id = id;
        }

    }
}
