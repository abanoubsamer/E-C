using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Result
{
    public class ResultUpdateStatusOrder : ResultServices
    {
        public OrderItem OrderItem { get; set; }
    }
}