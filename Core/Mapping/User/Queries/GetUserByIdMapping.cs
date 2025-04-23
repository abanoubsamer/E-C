using Core.Dtos;
using Core.Meditor.User.Queries.Response;
using Domain.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.User
{
    
    public partial class UserProfile
    {
        public void GetUserByIdMapping()
        {
         
            CreateMap<ApplicationUser, GetUserByIdReponse>()
                    .ForMember(des => des.id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(des => des.Picture, opt => opt.MapFrom(src => src.Picture))
                    .ForMember(des => des.DateCreate, opt => opt.MapFrom(src => src.DateCreate.ToString()))
                    .ForMember(des => des.countCard, opt => opt.MapFrom(src => src.card.Items.Count()))
                    .ForMember(des => des.ShippingAddresses, opt => opt.MapFrom(src => src.ShippingAddresses
                    .Select(x => new ShippingAddressesDto
                    {
                        AddressID = x.AddressID,
                        City = x.City,
                        Country = x.Country,
                        PostalCode = x.PostalCode,
                        State = x.State,
                        Street = x.Street,
                        HouseNumber = x.HouseNumber,
                        lat = x.lat,
                        lon = x.lon,
                        Suburb = x.Suburb,
                  
                        
                    }))).ForMember(des => des.PhoneNumberDtos, opt => opt.MapFrom(src => src.PhoneNumbers
                    .Select(x => new PhoneNumberDto
                    {
                        id = x.Id,
                        phoneNumber = x.PhoneNumber,
                    })));
        }
    }
}
