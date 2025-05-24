using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos;
using Domain.Dtos;

namespace Core.Meditor.Product.Queries.Response
{
    public class GetProductListResponseQueries
    {
        public string ProductID { get; set; }

        public string Name { get; set; }

        public string SUK { get; set; }

        public string Description { get; set; }

        public double AverageRating { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public SellerDto Seller { get; set; }

        public CategoryDto Category { get; set; }

        public List<ProductImagesDto> Images { get; set; }
    }
}