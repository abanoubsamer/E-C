using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.User
{
    public partial class UserProfile:Profile
    {
        public UserProfile()
        {
            GetUserByIdMapping();
            AddUserShippingAddressesMapping();
        }

    }
}
