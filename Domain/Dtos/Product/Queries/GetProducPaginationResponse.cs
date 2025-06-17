using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Dtos.Product.Queries
{
    public class GetProducPaginationResponse
    {
        public string ProductID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public double AverageRating { get; set; }

        public int StockQuantity { get; set; }

        public SellerDto Seller { get; set; }

        public string Images { get; set; }
    }
}