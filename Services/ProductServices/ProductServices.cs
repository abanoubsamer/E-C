﻿using Domain;
using Domain.Dtos;
using Domain.Dtos.Product.Queries;
using Domain.Models;
using Hangfire;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolWep.Data.Enums.Oredring;
using Services.FileSystemServices;
using Services.NotificationServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        #region Failds

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileServices;
        private readonly INotificationServices _notificationServices;

        #endregion Failds

        #region Constractor

        public ProductServices(IUnitOfWork unitOfWork, INotificationServices notificationServices, IFileServices fileServices)
        {
            _notificationServices = notificationServices;
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
        }

        #endregion Constractor

        #region Implemntation

        public async Task<ResultServices> AddProductAsync(ProductListing product, string categoryID, IFormFile MainImage, List<IFormFile> images)
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
                var masterProduct = await _unitOfWork.Repository<ProductMaster>().FindOneAsync(p => p.SKU == product.SKU);

                if (masterProduct == null)
                {
                    await _unitOfWork.Repository<ProductMaster>().AddAsync(product.Product);
                }

                uploadResult = await _fileServices.AddRangeImageProductAsync("wwwroot/images", MainImage, images);

                if (uploadResult.Succesd && uploadResult.Data != null)
                {
                    product.Images = uploadResult.Data.Select(img => new ProductImages
                    {
                        ImageUrl = img,
                        ProductID = product.ProductID
                    }).ToList();
                }
                else return uploadResult;

                product.ProductID = Guid.NewGuid().ToString();
                await _unitOfWork.Repository<ProductListing>().AddAsync(product);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommentAsync();

                return new ResultServices
                {
                    Succesd = true,
                    Msg = product.ProductID
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
                var prod = await _unitOfWork.Repository<ProductListing>().FindOneWithNoTrackingAsync(x => x.ProductID == Id);
                if (prod == null)
                    return new ResultServices() { Succesd = false, Msg = "Product Not Found" };

                var imageUrls = new List<string>();

                if (prod.Images.Any())
                {
                    imageUrls = prod.Images.Select(x => x.ImageUrl).ToList();
                }
                await _unitOfWork.Repository<ProductImages>().DeleteRangeAsync(prod.Images);

                await _unitOfWork.Repository<ProductListing>().DeleteAsync(prod);

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
                await _unitOfWork.RollBackAsync();
                return new ResultServices() { Succesd = false, Msg = ex.Message };
            }
        }

        public async Task<List<ProductListing>> GetAllProductAsync()
        {
            return await _unitOfWork.Repository<ProductListing>().GetAllAsyncWithNoTracking();
        }

        public async Task<ProductListing> GetProductByID(string Id)
        {
            return await _unitOfWork.Repository<ProductListing>().FindOneAsync(x => x.ProductID == Id);
        }

        public async Task<ResultServices> UpdateProductAsync(ProductListing product, IFormFile? MainImage, List<IFormFile>? Images, List<string>? IdImagesDeltetd)
        {
            if (product == null)
                return new ResultServices() { Msg = "Product Is Null" };

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingImages = await _unitOfWork.Repository<ProductImages>()
                    .FindMoreAsync(x => x.ProductID == product.ProductID);

                List<string> newImageUrls = new List<string>();
                if (MainImage != null)
                {
                    var uploadResult = await _fileServices.AddImageAsync("wwwroot/images", MainImage, true);
                    if (uploadResult.Succesd)
                    {
                        newImageUrls.Add(uploadResult.Msg);
                    }
                }
                if (Images != null && Images.Any())
                {
                    var uploadResult = await _fileServices.AddRangeImageAsync("wwwroot/images", Images);
                    if (uploadResult.Succesd && uploadResult.Data != null)
                    {
                        newImageUrls.AddRange(uploadResult.Data);
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

                await _unitOfWork.Repository<ProductListing>().UpdateAsync(product);
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
            var Query = GetQueryable(BrandId, ModelId, CategoryId);

            if (ProductName != null && ProductName != string.Empty)
            {
                Query = Query.Where(x => x.Name.Contains(ProductName));
            }

            return OrderBy(Query, orederBy, productOredringEnum);
        }

        private IQueryable<GetProducPaginationResponse> GetQueryable(string? BrandId, string? ModelId, string? CategoryId)
        {
            var query = _unitOfWork.Repository<ProductListing>()
                .GetQueryable()
                .AsNoTracking()
                .AsSplitQuery();

            if (!string.IsNullOrEmpty(ModelId))
            {
                query = query.Where(x => x.Product.Compatibilities.Any(c => c.ModelId == ModelId));
            }

            if (!string.IsNullOrEmpty(BrandId))
            {
                query = query.Where(x => x.Product.Compatibilities.Any(c => c.Model.BrandId == BrandId));
            }

            if (!string.IsNullOrEmpty(CategoryId))
            {
                query = query.Where(x => x.Product.CategoryID == CategoryId);
            }

            return query.Select(x => new GetProducPaginationResponse
            {
                AverageRating = x.Reviews.Any() ? x.Reviews.Average(r => (double?)r.Rating) ?? 0 : 0,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                ProductID = x.ProductID,
                StockQuantity = x.StockQuantity,
                Seller = new Domain.Dtos.SellerDto
                {
                    id = x.SellerID,
                    name = x.Seller.User.Name
                },
                Images = x.Images
                    .OrderByDescending(x => x.ImageUrl.StartsWith("main_"))
                    .Select(x => x.ImageUrl)
                    .FirstOrDefault()
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
            var product = await _unitOfWork.Repository<ProductListing>().FindOneWithNoTrackingAsync(x => x.ProductID == Id);

            return product.Price;
        }

        public async Task<List<ProductMasterDto>> GetMasterProduct(string SKU)
        {
            var master = await _unitOfWork.Repository<ProductMaster>().FindMoreAsNoTrackingAsync(x => x.SKU.Contains(SKU));
            if (master == null) return new List<ProductMasterDto>();
            return master.Select(e => new ProductMasterDto { CategoryID = e.CategoryID, SKU = e.SKU }).ToList();
        }

        public async Task<List<string>> SearchProductAsync(string ProductName)
        {
            if (string.IsNullOrEmpty(ProductName))
                return new List<string>();
            var productResult = await _unitOfWork.Repository<ProductListing>().FindMoreAsNoTrackingAsync(x => x.Name.Contains(ProductName));
            return productResult.Select(x => x.Name).ToList();
        }

        public async Task<bool> CheckStockAsync(string Id, int Quantity)
        {
            return await _unitOfWork.Repository<ProductListing>()
                .AnyAsync(x => x.ProductID == Id && x.StockQuantity >= Quantity);
        }

        public async Task<ResultServices> UpdateStockAsync(string Id, int Quantity)
        {
            var IsAvailable = await _unitOfWork.Repository<ProductListing>()
                 .AnyAsync(x => x.ProductID == Id && x.StockQuantity >= Quantity);
            if (IsAvailable == false) return new ResultServices() { Succesd = false, Msg = "Product Stock Not Available" };

            try
            {
                var product = await _unitOfWork.Repository<ProductListing>().FindOneAsync(x => x.ProductID == Id);

                product.StockQuantity -= Quantity;

                await _unitOfWork.Repository<ProductListing>().UpdateAsync(product);

                return new ResultServices() { Succesd = true, Msg = "Product Stock Updated" };
            }
            catch (Exception ex)
            {
                return new ResultServices() { Succesd = false, Msg = ex.Message };
            }
        }

        public async Task SendWeeklyReminderAndStartRecurring(string productId)
        {
            await _notificationServices.SendStockReminderNotificationAsync(productId);

            string jobId = $"stock_reminder_{productId}";
            RecurringJob.AddOrUpdate<INotificationServices>(
                jobId,
                x => x.SendStockReminderNotificationAsync(productId),
                Cron.Weekly);
        }

        #endregion Implemntation
    }
}