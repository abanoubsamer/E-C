using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums.Status
{
    public enum OrderItemStatus
    {
        Pending,
        Confirm,
        Shipped,    
        Delivered,  
        Cancelled   
    }
}
