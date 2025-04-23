using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Result
{
    public class AddOrderResult:ResultServices
    {
        public string orderID { get; set; }
        public decimal totalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string PaymentUrl { get; set; }
        
    }
}
