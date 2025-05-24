using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Seller.Quereis
{
    public class GetSelleProductsResponseDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public double avaragarate { get; set; }
        public string mainImage { get; set; }
    }
}