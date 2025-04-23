using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Result
{
    public class OrderWithTokenResult
    {
        public string Messgage { get; set; }

        public string Token { get; set; }

        public Order Order { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
