using Domain.Models;
using Microsoft.AspNetCore.Http;
using SchoolWep.Data.Enums.Oredring;
using Services.Result;
using System.Linq.Expressions;

namespace Services.CategoryServices
{
    public interface ICategoryServices
    {
        public Task<ResultServices> AddCategory(Category model, IFormFile image);

        public Task<ResultServices> UpdateCategory(Category model);

        public Task<ResultServices> DeleteCategory(string id);

        public Task<List<Category>> GetParentCategories();

        public Task<List<Category>> GetSubcategories(string id);

        public Task<Category> GetCategoryById(string id);

        public Expression<Func<Category, TResponse>> CreateExpression<TResponse>(Func<Category, TResponse> func);

        public IQueryable<Category> FilterCategory(string? CategoryName, OrederBy? orederBy);

        public Task<List<Category>> GetAllCategoriesdAsync();
    }
}