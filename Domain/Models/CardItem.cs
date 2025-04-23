using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CardItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
      
        [Required]
        public string ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public virtual Product Product { get; set; }

        public string CardId { get; set; }
        [ForeignKey(nameof(CardId))] 
        public virtual Card Card { get; set; }
        
        [Required]
        public int Quantity { get; set; }
       
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 
    }

}
