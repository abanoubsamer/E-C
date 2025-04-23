using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums.Status
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirm = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4,
    }
}
