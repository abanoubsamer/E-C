using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Auth
{
    public partial class AuthProfile: Profile
    {
        public AuthProfile()
        {
            AuthMapping();
            UserRegistrationMapping();
            SellerRegistrationMapping();
        }
    }
}
