using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Models
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }

        public int MinYear { get; set; }

        public int MaxYear { get; set; }

        public string Image { get; set; }

        public string BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public virtual CarBrand Brand { get; set; }

        public virtual ICollection<ModelCompatibility> ModelCompatibilities { get; set; } = new List<ModelCompatibility>();
    }
}