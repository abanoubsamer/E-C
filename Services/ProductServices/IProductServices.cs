using Domain.Dtos.Product.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using SchoolWep.Data.Enums.Oredring;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public interface IProductServices
    {
        public Task<List<Product>> GetAllProductAsync();

        public Task<ResultServices> AddProductAsync(Product product, List<IFormFile> Images);

        public Task<ResultServices> DeleteProductAsync(string Id);

        public Task<ResultServices> UpdateProductAsync(Product product, List<IFormFile>? Images, List<string>? IdImagesDeltetd);

        public Task<Product> GetProductByID(string Id);

        public Task<decimal> GetProductPriceByID(string Id);

        public IQueryable<GetProducPaginationResponse> FilterStudent(string? ProductName, string? BrandId, string? ModelId, string? CategoryId, OrederBy? orederBy, ProductOredringEnum? productOredringEnum);
    }
}