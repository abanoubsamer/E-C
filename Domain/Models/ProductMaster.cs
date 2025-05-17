using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductMaster
    {
        [Key]
        public string SKU { get; set; } // المفتاح الأساسي

        public string CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public virtual Category Category { get; set; }

        public virtual ICollection<ModelCompatibility> Compatibilities { get; set; }

        public virtual ICollection<ProductListing> Listings { get; set; }
    }
}