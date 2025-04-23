using Core.Dtos;
using Domain.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Response
{
    public class GetUserPaginationReponse
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public  List<ShippingAddressesDto>? ShippingAddresses { get; set; }
       
        public GetUserPaginationReponse(Domain.Models.ApplicationUser user)
        {
            id = user.Id;
            Name = user.Name;
            Email = user.Email;
            ShippingAddresses = user.ShippingAddresses.Select(x => new ShippingAddressesDto
            {
                AddressID = x.AddressID,
                City = x.City,
                Country = x.Country,
                PostalCode = x.PostalCode,
                State = x.State,
                Street=x.Street,       
            }).ToList();
        }

    }

 
  

}
