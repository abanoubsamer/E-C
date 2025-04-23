using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Queries.Response
{
    public class GetProductPaginagtionResponseQueries: GetProductListResponseQueries
    {

       
            public GetProductPaginagtionResponseQueries(Domain.Models.Product product)
            {
                ProductID = product.ProductID.ToString();
                Name = product.Name;
                Description = product.Description;
         
                AverageRating = product.AverageRating;
                Price = product.Price;
                StockQuantity = product.StockQuantity;

                Seller = new SellerDto
                {
                    id = product.Seller.SellerID.ToString(),
                    name = product.Seller.User.Name
                };

                Category = new CategoryDto
                {
                    Id = product.Category.CategoryID.ToString(),
                    Name = product.Category.Name
                };

                Images = product.Images.Select(img => new ProductImagesDto
                {
                    id = img.ImageID.ToString(),
                    Image = img.ImageUrl
                }).ToList();
            }
        }

    
}
