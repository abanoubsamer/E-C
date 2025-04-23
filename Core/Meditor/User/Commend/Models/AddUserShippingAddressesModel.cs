using Core.Basic;
using Core.Dtos;
using Core.Dtos.Commend.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Commend.Models
{
    public class AddUserShippingAddressesModel:IRequest<Response<string>> 
    {
        public string? UserId { get; set; }
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
