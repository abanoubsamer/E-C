using Core.Basic;
using Core.Meditor.User.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Queries.Model
{
    public class RefrashTokenModelQueries:IRequest<Response<AuthResponseQueries>>
    {

        public RefrashTokenModelQueries(string refreshtoken, string accesstoken)
        {
            RefreshToken = refreshtoken;
            AccessToken = accesstoken;
        }

        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}
