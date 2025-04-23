using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class PaymentDto
    {
       
        public string PaymentMethod { get; set; }
        
        public string TransactionID { get; set; }

        public decimal Amount { get; set; }
               
    }
}
