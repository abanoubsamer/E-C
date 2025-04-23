using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Order
{
    public partial class OrderProfile : Profile
    {

        public OrderProfile()
        {
            GetUserOrders();
        }

    }
}
