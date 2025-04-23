using Core.Dtos;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Response
{
    public class GetUserByIdReponse
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DateCreate { get; set; } 
        public int countCard { get; set; }
        public string? Picture { get; set; }
        public List<ShippingAddressesDto>? ShippingAddresses { get; set; }
        public List<PhoneNumberDto>? PhoneNumberDtos { get; set; }
    }
}
