using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Commend.Model
{
    public class RegistrationUserModelCommend:IRequest<Response<string>>
    {
       
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ComperPassword { get; set; }

    }
}
