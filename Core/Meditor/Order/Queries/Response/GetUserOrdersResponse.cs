using Core.Dtos;
using Core.Dtos.Product.Queries;
using Domain.Dtos;
using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Queries.Response
{
    public class GetUserOrdersResponse
    {

        public string OrderID { get; set; }

        public string OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } 

        public virtual PaymentDto Payment { get; set; }

        public virtual PhoneNumberDto Phone { get; set; }

        public virtual ShippingAddressesDto shippingAddresses { get; set; }

        public virtual List<GetOrderItemsWithOrderDto> OrderItems { get; set; }
    }
}
