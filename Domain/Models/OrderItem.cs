using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderItem
    {
        [Required]
        public string OrderID { get; set; }

        public virtual Order Order { get; set; }

        [Required]
        public string ProductID { get; set; }

        public virtual ProductListing Product { get; set; }

        [Required]
        public OrderItemStatus Status { get; set; }

        [Required]
        public string SellerID { get; set; }

        [ForeignKey(nameof(SellerID))]
        public virtual Seller Seller { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}