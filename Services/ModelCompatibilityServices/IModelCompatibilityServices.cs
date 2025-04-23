using Domain.Models;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ModelCompatibilityServices
{
    public interface IModelCompatibilityServices
    {
        public Task<ResultServices> AddModelCompatibilityAsync(ModelCompatibility entity);

        public Task<ResultServices> UpdateModelCompatibilityAsync(ModelCompatibility entity);

        public Task<ResultServices> DeleteModelCompatibilityAsync(ModelCompatibility entity);

        public Task<ResultServices> AddModelCompatibilityRangeAsync(List<ModelCompatibility> entity);

        public Task<ResultServices> DeleteModelCompatibilityRangeAsync(List<ModelCompatibility> entity);

        public Task<ModelCompatibility> GetModelCompatibilityByIdAsync(string id);

        public IQueryable<ModelCompatibility> GetModelCompatibilities(string ProductId);
    }
}