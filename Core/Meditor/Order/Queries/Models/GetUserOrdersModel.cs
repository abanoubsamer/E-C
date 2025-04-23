using Core.Basic;
using Core.Meditor.Order.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Queries.Models
{
    public class GetUserOrdersModel:IRequest<Response<List<GetUserOrdersResponse>>>
    {

        public string Id { get; set; }
        public GetUserOrdersModel(string id)
        {
            Id = id;
        }

    }
}
