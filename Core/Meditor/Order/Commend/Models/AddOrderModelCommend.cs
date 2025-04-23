using Core.Basic;
using Core.Dtos;
using Core.Meditor.Order.Commend.Response;
using Domain.Dtos;
using Domain.Enums.Status;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Commend.Models
{
    public class JWTOrderModelCommend : IRequest<Response<string>>
    {

        [Required]
        public string UserID { get; set; }
        public string phoneId { get; set; }
        public string shippingAddressID { get; set; }
       
        
        [Required]
        public List<OrderItemsDto> orderItems { get; set; }



    }
}
