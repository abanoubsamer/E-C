using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductInteraction
    {
        [Key]
        public string Id { get; set; }

        public string ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual ProductListing Product { get; set; }

        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        public string InteractionType { get; set; } = string.Empty; // AddedToCart, Purchased, etc.
        public DateTime InteractionDate { get; set; }
    }
}