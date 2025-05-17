using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Owned]
    public class ProductImages
    {
        [Key]
        public string ImageID { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string ProductID { get; set; }

        [ForeignKey(nameof(ProductID))]
        public virtual ProductListing Product { get; set; }
    }
}