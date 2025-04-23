using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Commend.Model
{
    public class RegistrationSellerModelCommend : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AccountType { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string ComperPassword { get; set; }
        public string ShopName { get; set; }
        public string ContactInfo { get; set; }
    }
}
