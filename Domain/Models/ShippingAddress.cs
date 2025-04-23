using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ShippingAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AddressID { get; set; }
        
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        ApplicationUser User { get; set; }

        public string Country { get; set; }
        public string? Street {  get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Suburb { get; set; }
        public string? PostalCode { get; set; }
        public string? HouseNumber { get; set; }
        public string? lat { get; set; }
        public string? lon { get; set; }
    }
}
