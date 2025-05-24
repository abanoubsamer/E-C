using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Product.Commend
{
    public class AddOrderTestDtos
    {
        public string UserID { get; set; }
        public string shippingAddressId { get; set; }
        public string phoneNumberId { get; set; }
        public PaymentDto paymentMethod { get; set; }
        public List<OrderItemsDto> orderItems { get; set; }
    }
}