using Core.Basic;
using Core.Meditor.User.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Modles
{
    public class GetUserByIdModel:IRequest<Response<GetUserByIdReponse>>
    {
        public string Id { get; set; }

        public GetUserByIdModel(string id)
        {
            Id = id;
        }

    }
}
