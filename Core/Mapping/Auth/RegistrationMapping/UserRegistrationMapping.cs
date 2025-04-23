using Core.Meditor.User.Commend.Model;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Auth
{
    public partial class AuthProfile
    {

        public void UserRegistrationMapping()
        {
            CreateMap<RegistrationUserModelCommend, ApplicationUser>()
                 .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(des => des.Name, opt => opt.MapFrom(src => src.UserName))
                 .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
        }


    }
}
