using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ModelCompatibility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public int MinYear { get; set; }

        public int MaxYear { get; set; }

        public string ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public virtual Models Model { get; set; }

        public string SKU { get; set; }

        [ForeignKey(nameof(SKU))]
        public virtual ProductMaster ProductMaster { get; set; }
    }
}