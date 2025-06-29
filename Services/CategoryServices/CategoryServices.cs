﻿using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SchoolWep.Data.Enums.Oredring;
using Services.ExtinsionServies;
using Services.FileSystemServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        #region Failds

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileServices;
        private readonly IMemoryCache _cache;

        #endregion Failds

        #region Constractor

        public CategoryServices(IUnitOfWork unitOfWork, IMemoryCache cache, IFileServices fileServices)
        {
            _cache = cache;
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
        }

        #endregion Constractor

        public async Task<ResultServices> AddCategory(Category model, IFormFile image)
        {
            if (model == null) return new ResultServices { Msg = " Invalid Category" };
            if (image == null) return new ResultServices { Msg = "Invalid Image" };
            try
            {
                var ImageResult = await _fileServices.AddImageAsync("wwwroot/images/Category", image);
                if (!ImageResult.Succesd) return ImageResult;
                model.Image = ImageResult.Msg;
                await _unitOfWork.Repository<Category>().AddAsync(model);
                return new ResultServices
                {
                    Succesd = true,
                };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<ResultServices> DeleteCategory(string id)
        {
            if (string.IsNullOrEmpty(id)) return new ResultServices { Msg = "Invalid Id" };

            try
            {
                var category = await _unitOfWork.Repository<Category>().FindOneAsync(x => x.CategoryID == id);
                if (category == null) return new ResultServices { Msg = "Not Found Category" };

                await _unitOfWork.Repository<Category>().DeleteAsync(category);
                return new ResultServices
                {
                    Succesd = true,
                };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<ResultServices> UpdateCategory(Category model)
        {
            if (model == null) return new ResultServices { Msg = "Invalid Category" };
            try
            {
                await _unitOfWork.Repository<Category>().UpdateAsync(model);
                return new ResultServices
                {
                    Succesd = true,
                };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<List<Category>> GetAllCategoriesdAsync()
        {
            const string cacheKey = "AllCategories";

            if (_cache.TryGetValue(cacheKey, out List<Category> cachedCategories))
                return cachedCategories;

            var result = await _unitOfWork.Repository<Category>().GetQueryable()
                .Where(x => x.SubCategories.Any())
                .Include(x => x.SubCategories)
                .OrderBy(x => x.Name)
                .ToListAsync();

            _cache.Set(cacheKey, result, TimeSpan.FromHours(2));

            return result;
        }

        public IQueryable<Category> FilterCategory(string? CategoryName, OrederBy? orederBy)
        {
            var Query = GetQueryable();
            if (!CategoryName.IsNullOrEmpty())
            {
                Query = Query.Where(x => x.Name.Contains(CategoryName));
            }

            bool ascending = orederBy == null || orederBy == 0;

            return ascending ? Query.OrderBy(x => x.Name) : Query.OrderByDescending(x => x.Name);
        }

        public Expression<Func<Category, TResponse>> CreateExpression<TResponse>(Func<Category, TResponse> func)
        {
            return e => func(e);
        }

        private IQueryable<Category> GetQueryable()
        {
            return _unitOfWork.Repository<Category>().GetQueryable()
                  .Where(x => x.SubCategories.Any())
                    .Include(x => x.SubCategories);
        }

        public async Task<Category> GetCategoryById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            string cacheKey = $"Category_{id}";

            if (_cache.TryGetValue(cacheKey, out Category cachedCategory))
                return cachedCategory;

            var category = await _unitOfWork.Repository<Category>()
                .FindOneAsync(x => x.CategoryID == id);

            if (category != null)
                _cache.Set(cacheKey, category, TimeSpan.FromHours(1));

            return category;
        }

        public async Task<List<Category>> GetParentCategories()
        {
            return await _unitOfWork.Repository<Category>().FindMoreAsNoTrackingAsync(x => x.ParentCategory == null);
        }

        public async Task<List<Category>> GetSubcategories(string id)
        {
            return await _unitOfWork.Repository<Category>().FindMoreAsNoTrackingAsync(x => x.ParentCategoryID == id);
        }
    }
}