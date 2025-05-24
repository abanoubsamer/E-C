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
    public class GetsellerProductByIdModel : IRequest<Response<GetSellerProductByIdResponse>>
    {
        public string Id { get; set; }

        public GetsellerProductByIdModel(string id)
        {
            Id = id;
        }
    }
}