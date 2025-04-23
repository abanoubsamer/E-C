using Core.Meditor.Authentication.Commend.Model;
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

        public void SellerRegistrationMapping()
        {

            CreateMap<RegistrationSellerModelCommend, ApplicationUser>()
                .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Seller, opt => opt.MapFrom(src =>
                    new Domain.Models.Seller
                    {
                        ContactInfo = src.ContactInfo,
                        country = src.Country,
                        Type = src.AccountType,                    
                        SellerID = Guid.NewGuid().ToString(),
                        ShopName = src.ShopName
                    }
                ))
                .AfterMap((src, des) =>
                {
                    des.Seller.UserID = des.Id; 
                    des.SellerID = des.Seller.SellerID; 
                });
        }
    }
}
