using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Queries.Response
{
    public class GetSellerProductByIdResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string descreption { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public List<ProductImagesDto> ProductImagesDto { get; set; }
        public CategoryDto category { get; set; }
        public List<GetModelCompatibilitySellerDto> modelCompatibilityDto { get; set; }
    }

    public class GetModelCompatibilitySellerDto
    {
        public string Id { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string ModelName { get; set; }
        public string BrandName { get; set; }
    }
}