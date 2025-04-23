using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Services.FileSystemServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ModelsServices
{
    public class ModelsServices : IModelsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices fileServices;

        public ModelsServices(IUnitOfWork unitOfWork, IFileServices fileServices)
        {
            _unitOfWork = unitOfWork;
            this.fileServices = fileServices;
        }

        public async Task<ResultServices> AddModel(Models entity, IFormFile image)
        {
            if (entity == null) return new ResultServices { Msg = "Model is null", Succesd = false };
            if (image == null) return new ResultServices { Msg = "Image is null", Succesd = false };
            try
            {
                var resultimage = await fileServices.AddImageAsync("wwwroot/images/Models", image);
                if (!resultimage.Succesd) return new ResultServices { Msg = resultimage.Msg, Succesd = false };
                entity.Image = resultimage.Msg;
                await _unitOfWork.Repository<Models>().AddAsync(entity);

                return new ResultServices { Msg = "Model added successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<ResultServices> AddModelRange(List<Models> entity)
        {
            if (!entity.Any()) return new ResultServices { Msg = "Model is null", Succesd = false };
            try
            {
                await _unitOfWork.Repository<Models>().AddRangeAsync(entity);
                return new ResultServices { Msg = "Model added successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<Models> GetModelById(string id)
        {
            if (id == null) return null;
            return await _unitOfWork.Repository<Models>().FindOneAsync(x => x.Id == id);
        }

        public async Task<ResultServices> RemoveModel(Models entity)
        {
            if (entity == null) return new ResultServices { Msg = "Model is null", Succesd = false };
            try
            {
                await _unitOfWork.Repository<Models>().DeleteAsync(entity);
                return new ResultServices { Msg = "Model removed successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<ResultServices> RemoveModelRange(List<Models> entity)
        {
            if (!entity.Any()) return new ResultServices { Msg = "Model is null", Succesd = false };
            try
            {
                await _unitOfWork.Repository<Models>().DeleteRangeAsync(entity);
                return new ResultServices { Msg = "Model removed successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<ResultServices> UpdateModel(Models entity)
        {
            if (entity == null) return new ResultServices { Msg = "Model is null", Succesd = false };
            try
            {
                await _unitOfWork.Repository<Models>().UpdateAsync(entity);
                return new ResultServices { Msg = "Model updated successfully", Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message, Succesd = false };
            }
        }

        public async Task<List<Models>> GetModelsWithCarBrand(string CarBrandId)
        {
            return await _unitOfWork.Repository<Models>().FindMoreAsNoTrackingAsync(x => x.BrandId == CarBrandId);
        }
    }
}