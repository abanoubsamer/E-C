using Domain.Dtos.Product.Queries;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolWep.Data.Enums.Oredring;
using Services.FileSystemServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        #region Failds

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileServices;

        #endregion Failds

        #region Constractor

        public ProductServices(IUnitOfWork unitOfWork, IFileServices fileServices)
        {
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
        }

        #endregion Constractor

        #region Implemntation

        public async Task<ResultServices> AddProductAsync(Product product, List<IFormFile> images)
        {
            if (product == null)
            {
                return new ResultServices
                {
                    Msg = "Product is null",
                    Succesd = false
                };
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            var uploadResult = new FileSystemResult();
            try
            {
                uploadResult = await _fileServices.AddRangeImageAsync("wwwroot/images", images);

                if (uploadResult.Succesd && uploadResult.Data != null)
                {
                    product.Images = uploadResult.Data.Select(img => new ProductImages
                    {
                        ImageUrl = img,
                        ProductID = product.ProductID
                    }).ToList();
                }
                else return uploadResult;

                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommentAsync();

                return new ResultServices
                {
                    Succesd = true
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                await _fileServices.DeleteRangeImageAsync(uploadResult.Data);
                return new ResultServices
                {
                    Msg = ex.Message,
                    Succesd = false
                };
            }
        }

        public async Task<ResultServices> DeleteProductAsync(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return new ResultServices { Succesd = false, Msg = "ID is Invalid" };

            using var transaction = await _unitOfWork.BeginTransactionAsync(); // Ensure transaction is used here
            try
            {
                var prod = await _unitOfWork.Repository<Product>().FindOneWithNoTrackingAsync(x => x.ProductID == Id);
                if (prod == null)
                    return new ResultServices() { Succesd = false, Msg = "Product Not Found" };

                var imageUrls = new List<string>();

                if (prod.Images.Any())
                {
                    imageUrls = prod.Images.Select(x => x.ImageUrl).ToList();
                }
                await _unitOfWork.Repository<ProductImages>().DeleteRangeAsync(prod.Images);

                await _unitOfWork.Repository<Product>().DeleteAsync(prod);

                if (imageUrls.Any())
                {
                    var result = await _fileServices.DeleteRangeImageAsync(imageUrls);
                    if (!result.Succesd)
                    {
                        await _unitOfWork.RollBackAsync(); // Ensure rollback in case of failure
                        return new ResultServices() { Succesd = false, Msg = result.Msg }; // Return failure message from image deletion
                    }
                }

                await _unitOfWork.CommentAsync();

                return new ResultServices() { Succesd = true };
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of an exception
                await _unitOfWork.RollBackAsync();
                return new ResultServices() { Succesd = false, Msg = ex.Message };
            }
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _unitOfWork.Repository<Product>().GetAllAsyncWithNoTracking();
        }

        public async Task<Product> GetProductByID(string Id)
        {
            return await _unitOfWork.Repository<Product>().FindOneAsync(x => x.ProductID == Id);
        }

        public async Task<ResultServices> UpdateProductAsync(Product product, List<IFormFile>? Images, List<string>? IdImagesDeltetd)
        {
            if (product == null)
                return new ResultServices() { Msg = "Product Is Null" };

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingImages = await _unitOfWork.Repository<ProductImages>()
                    .FindMoreAsync(x => x.ProductID == product.ProductID);

                List<string> newImageUrls = new List<string>();

                if (Images != null && Images.Any())
                {
                    var uploadResult = await _fileServices.AddRangeImageAsync("wwwroot/images", Images);
                    if (uploadResult.Succesd && uploadResult.Data != null)
                    {
                        newImageUrls = uploadResult.Data;
                    }
                }

                if (IdImagesDeltetd != null && IdImagesDeltetd.Any())
                {
                    var imagesToDelete = existingImages
                        .Where(img => IdImagesDeltetd.Contains(img.ImageID.ToString()))
                        .ToList();

                    if (imagesToDelete.Any())
                    {
                        var deleteResult = await _fileServices.DeleteRangeImageAsync(imagesToDelete.Select(x => x.ImageUrl).ToList());
                        if (deleteResult.Succesd)
                        {
                            await _unitOfWork.Repository<ProductImages>().DeleteRangeAsync(imagesToDelete);
                        }
                    }
                }

                var imagesToAdd = newImageUrls
                    .Select(img => new ProductImages
                    {
                        ImageUrl = img,
                        ProductID = product.ProductID
                    }).ToList();

                if (imagesToAdd.Any())
                {
                    await _unitOfWork.Repository<ProductImages>().AddRangeAsync(imagesToAdd);
                }

                await _unitOfWork.Repository<Product>().UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommentAsync();

                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return new ResultServices
                {
                    Msg = ex.Message,
                    Succesd = false
                };
            }
        }

        public IQueryable<GetProducPaginationResponse> FilterStudent(string? ProductName, string? BrandId, string? ModelId, string? CategoryId, OrederBy? orederBy, ProductOredringEnum? productOredringEnum)
        {
            var Query = GetQueryable();

            if (ProductName != null && ProductName != string.Empty)
            {
                Query = Query.Where(x => x.Name.Contains(ProductName));
            }
            if (CategoryId != null && CategoryId != string.Empty)
            {
                Query = Query.Where(x => x.Category.Id.Contains(CategoryId));
            }
            if (BrandId != null && BrandId != string.Empty)
            {
                Query = Query.Where(x => x.BrandCompatibility.Any(b => b == BrandId));
            }
            if (ModelId != null && ModelId != string.Empty)
            {
                Query = Query.Where(x => x.ModelCompatibility.Any(m => m == ModelId));
            }

            return OrderBy(Query, orederBy, productOredringEnum);
        }

        private IQueryable<GetProducPaginationResponse> GetQueryable()
        {
            return _unitOfWork.Repository<Product>()
             .GetQueryable().AsNoTracking().AsSplitQuery()
             .Select(x => new GetProducPaginationResponse
             {
                 AverageRating = x.Reviews.Select(r => (double?)r.Rating).Average() ?? 0,
                 Description = x.Description,
                 Category = new Domain.Dtos.CategoryDto { Name = x.Category.CategoryID, Id = x.CategoryID },
                 Name = x.Name,
                 Price = x.Price,
                 ProductID = x.ProductID,
                 StockQuantity = x.StockQuantity,
                 Seller = new Domain.Dtos.SellerDto { id = x.SellerID, name = x.Seller.User.Name },
                 Images = x.Images.Select(x => new Domain.Dtos.ProductImagesDto
                 {
                     id = x.ImageID,
                     Image = x.ImageUrl
                 }).ToList(),
                 ModelCompatibility = x.ModelCompatibilities.Select(x => x.ModelId).ToList(),
                 BrandCompatibility = x.ModelCompatibilities.Select(x => x.Model.BrandId).Distinct().ToList()
             });
        }

        private IQueryable<GetProducPaginationResponse> OrderBy(IQueryable<GetProducPaginationResponse> Query, OrederBy? orederBy, ProductOredringEnum? productOredringEnum)
        {
            productOredringEnum = productOredringEnum == null ? 0 : productOredringEnum;

            bool ascending = orederBy == null || orederBy == 0;

            switch (productOredringEnum)
            {
                case ProductOredringEnum.Name:
                    Query = ascending ? Query.OrderBy(x => x.Name) : Query.OrderByDescending(x => x.Name);
                    break;

                case ProductOredringEnum.Price:
                    Query = ascending ? Query.OrderBy(x => x.Price) : Query.OrderByDescending(x => x.Price);
                    break;

                case ProductOredringEnum.Rating:
                    Query = ascending ? Query.OrderBy(x => x.AverageRating) : Query.OrderByDescending(x => x.AverageRating);
                    break;
            }

            return Query;
        }

        public async Task<decimal> GetProductPriceByID(string Id)
        {
            var product = await _unitOfWork.Repository<Product>().FindOneWithNoTrackingAsync(x => x.ProductID == Id);

            return product.Price;
        }

        #endregion Implemntation
    }
}