using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderID { get; set; }  

        [Required]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }


        [NotMapped]
        public OrderStatus Status
        {
            get
            {
                if (OrderItems.All(oi => oi.Status == OrderItemStatus.Delivered))
                    return OrderStatus.Delivered;
                if (OrderItems.All(oi => oi.Status == OrderItemStatus.Cancelled))
                    return OrderStatus.Cancelled;
                if (OrderItems.Any(oi => oi.Status == OrderItemStatus.Shipped))
                    return OrderStatus.Shipped;
                return OrderStatus.Pending;
            }
        }
        public virtual Payment Payment { get; set; }



        [Required]
        public string AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public virtual ShippingAddress ShippingAddress { get; set; }



        [Required]
        public string PhoneId { get; set; }
        [ForeignKey(nameof(PhoneId))]
        public virtual UserPhoneNumber UserPhoneNumber { get; set; }


        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }


}
