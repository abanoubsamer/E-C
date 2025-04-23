using Core.Basic;
using Core.Meditor.Card.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Queries.Models
{
    public class GetCardUserModelQueries:IRequest<Response<GetCardUserRespones>>
    {

        public string _id { get; set; }
        public GetCardUserModelQueries(string id)
        {
            _id = id;   
        }

    }
}
