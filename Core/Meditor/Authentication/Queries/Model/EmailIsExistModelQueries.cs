using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Queries.Model
{
    public class EmailIsExistModelQueries:IRequest<Response<bool>>
    {
        public string _email {  get; set; }

        public EmailIsExistModelQueries(string email)
        {
            _email = email;
        }
    }
}
