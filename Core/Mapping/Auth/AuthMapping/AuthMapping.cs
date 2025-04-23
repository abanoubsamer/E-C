using Core.Meditor.User.Queries.Response;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Auth
{
    public partial class AuthProfile
    {

        public void AuthMapping()
        {
            CreateMap<AuthModelResult, AuthResponseQueries>()
               .ForMember(des => des.UserID, src => src.MapFrom(src => src.UserId))
               .ForMember(des => des.Username, src => src.MapFrom(src => src.UserName))
                 .ForMember(des => des.Token, src => src.MapFrom(src => src.Token))
                   .ForMember(des => des.Expiration, src => src.MapFrom(src => src.Expiration))
                     .ForMember(des => des.Roles, src => src.MapFrom(src => src.Roles));
        }

    }
}
