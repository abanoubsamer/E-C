using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

namespace Core.Dtos.Product.Queries
{
    public class GetProductDto
    {
        public string ProductID { get; set; }

        public string Name { get; set; }
       
        public string Description { get; set; }

        public decimal Price { get; set; }

        public  CategoryDto Category { get; set; }

        public double AverageRating { get; set; }

        public  List<ProductImagesDto> ImagesDto { get; set; } 
    }
}
