using Domain.Models;
using Infrastructure.UnitOfWork;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ModelCompatibilityServices
{
    public class ModelCompatibilityServices : IModelCompatibilityServices
    {
        private readonly IUnitOfWork unitOfWork;

        public ModelCompatibilityServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResultServices> AddModelCompatibilityAsync(ModelCompatibility entity)
        {
            if (entity == null) return new ResultServices { Msg = "Model Compatibility is null", Succesd = false };
            try
            {
                await unitOfWork.Repository<ModelCompatibility>().AddAsync(entity);

                return new ResultServices { Msg = "Model Compatibility added successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<ResultServices> AddModelCompatibilityRangeAsync(List<ModelCompatibility> entity)
        {
            if (!entity.Any()) return new ResultServices { Msg = "Model Compatibility is null", Succesd = false };

            try
            {
                await unitOfWork.Repository<ModelCompatibility>().AddRangeAsync(entity);
                return new ResultServices { Msg = "Model Compatibility added successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<ResultServices> DeleteModelCompatibilityAsync(ModelCompatibility entity)
        {
            if (entity == null) return new ResultServices { Msg = "Model Compatibility is null", Succesd = false };
            try
            {
                await unitOfWork.Repository<ModelCompatibility>().DeleteAsync(entity);
                return new ResultServices { Msg = "Model Compatibility deleted successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<ResultServices> DeleteModelCompatibilityRangeAsync(List<ModelCompatibility> entity)
        {
            if (!entity.Any()) return new ResultServices { Msg = "Model Compatibility is null", Succesd = false };
            try
            {
                await unitOfWork.Repository<ModelCompatibility>().DeleteRangeAsync(entity);
                return new ResultServices { Msg = "Model Compatibility deleted successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public IQueryable<ModelCompatibility> GetModelCompatibilities(string ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelCompatibility> GetModelCompatibilityByIdAsync(string id)
        {
            return await this.unitOfWork.Repository<ModelCompatibility>().FindOneAsync(x => x.Id == id);
        }

        public async Task<ResultServices> UpdateModelCompatibilityAsync(ModelCompatibility entity)
        {
            if (entity == null) return new ResultServices { Msg = "Model Compatibility is null", Succesd = false };
            try
            {
                await unitOfWork.Repository<ModelCompatibility>().UpdateAsync(entity);
                return new ResultServices { Msg = "Model Compatibility updated successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }
    }
}