using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class GetSellerOrderDto
    {
        public string OrderID { get; set; }
        public UserDto User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderItemStatus Status { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; }
        public ShippingAddressesDto ShippingAddresses { get; set; }
    }
}
