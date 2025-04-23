using Core.Meditor.Seller.Queries.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Seller
{
    public partial class SellerProfile
    {
       public void GetSellerById()
        {
            CreateMap<Domain.Models.Seller, GetSellerByIdResponse>()
                .ForMember(des => des.email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(des => des.country, opt => opt.MapFrom(src => src.country))
                .ForMember(des => des.type, opt => opt.MapFrom(src => src.Type))
                .ForMember(des => des.dateCreate, opt => opt.MapFrom(src => src.User.DateCreate.ToString()))
                .ForMember(des => des.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ForMember(des => des.id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(des => des.sellerId, opt => opt.MapFrom(src => src.SellerID))
                .ForMember(des => des.name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(des => des.picture, opt => opt.MapFrom(src => src.User.Picture))
                .ForMember(des => des.ShopName, opt => opt.MapFrom(src => src.ShopName));
        }
    }
}
