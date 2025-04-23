using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Commend.Model
{
    public class ValidationTokenCommend:IRequest<Response<string>>
    {
        public string Token { get; set; }
   
    }
}
