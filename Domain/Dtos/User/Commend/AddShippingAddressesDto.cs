using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.User.Commend
{
    public class AddShippingAddressesDto
    {
        public string UserId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string HouseNumber { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
    }
}
