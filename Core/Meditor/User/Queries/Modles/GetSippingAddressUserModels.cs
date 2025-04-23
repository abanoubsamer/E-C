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
    public class GetSippingAddressUserModels : IRequest<Response<List<GetSippingAddressUserResponse>>>
    {
        public string userId { get; set; }
        public GetSippingAddressUserModels(string userId) {
        
            this.userId= userId;
        
        }


    }
}
