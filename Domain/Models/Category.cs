using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CategoryID { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        public string Image { get; set; }

        // Self-referencing for subcategories
        public string? ParentCategoryID { get; set; }

        [ForeignKey(nameof(ParentCategoryID))]
        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> SubCategories { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}