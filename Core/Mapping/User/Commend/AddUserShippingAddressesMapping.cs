using Core.Dtos;
using Core.Dtos.Commend.User;
using Core.Meditor.User.Commend.Models;
using Domain.Dtos;
using Domain.Models;
using Services.ExtinsionServies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.User
{
    public partial class UserProfile
    {
        public void AddUserShippingAddressesMapping()
        {
        CreateMap<ShippingAddressesDto, ShippingAddress>()
                .ForMember(des => des.AddressID, opt => opt.Condition(src => !src.AddressID.IsNullOrEmpty()))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.City))
                .ForMember(des => des.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(des => des.State, opt => opt.MapFrom(src => src.State))
                .ForMember(des => des.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
                .ForMember(des => des.Suburb, opt => opt.MapFrom(src => src.Suburb))
                .ForMember(des => des.lat, opt => opt.MapFrom(src => src.lat))
                .ForMember(des => des.lon, opt => opt.MapFrom(src => src.lon))
                .ForMember(des => des.PostalCode, opt => opt.MapFrom(src => src.PostalCode)).ReverseMap();


            CreateMap<AddUserShippingAddressesModel, ShippingAddress>()
               .ForMember(des => des.ApplicationUserId, opt => opt.MapFrom(src => src.UserId))
               .ForMember(des => des.AddressID, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
               .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Street))
               .ForMember(des => des.City, opt => opt.MapFrom(src => src.City))
               .ForMember(des => des.Country, opt => opt.MapFrom(src => src.Country))
               .ForMember(des => des.State, opt => opt.MapFrom(src => src.State))
               .ForMember(des => des.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
               .ForMember(des => des.Suburb, opt => opt.MapFrom(src => src.Suburb))
               .ForMember(des => des.lat, opt => opt.MapFrom(src => src.lat))
               .ForMember(des => des.lon, opt => opt.MapFrom(src => src.lon))
               .ForMember(des => des.PostalCode, opt => opt.MapFrom(src => src.PostalCode)).ReverseMap();

        }
    }
}
