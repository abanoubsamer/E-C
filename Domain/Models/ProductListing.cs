using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductListing
    {
        [Key]
        public string ProductID { get; set; }

        [Required]
        public string SKU { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(SKU))]
        public virtual ProductMaster Product { get; set; }

        public string SellerID { get; set; }

        [ForeignKey(nameof(SellerID))]
        public virtual Seller Seller { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        [NotMapped]
        public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;

        public virtual ICollection<ProductImages> Images { get; set; } = new List<ProductImages>();
    }
}