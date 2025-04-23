using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums.Status;

namespace Domain.Dtos
{
    public class OrderItemsDto
    {
        public string ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal? price { get; set; }

        public OrderItemStatus? Status { get; set; }
    }
}
