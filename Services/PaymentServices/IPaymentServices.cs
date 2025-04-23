using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentServices
{
    public interface IPaymentServices
    {
        public Task<string> GetURLPaymentAsync(Order order, string authToken);
    }
}
