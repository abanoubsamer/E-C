using Core.Basic;
using Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Commend.Models
{
    public class AddOrderTestModel : IRequest<Response<string>>
    {
        public string UserID { get; set; }
        public string shippingAddressId { get; set; }
        public string phoneNumberId { get; set; }
        public PaymentDto paymentMethod { get; set; }
        public List<OrderItemsDto> orderItems { get; set; }
    }
}