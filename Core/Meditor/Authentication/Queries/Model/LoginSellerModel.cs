using Core.Basic;
using Core.Meditor.User.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Queries.Model
{
    public class LoginSellerModel:IRequest<Response<AuthResponseQueries>>
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
