using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Services.FileSystemServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Services.CarBrandServices
{
    public class CarBrandServices : ICarBrandServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileServices fileServices;

        public CarBrandServices(IUnitOfWork unitOfWork, IFileServices fileServices)
        {
            this.unitOfWork = unitOfWork;
            this.fileServices = fileServices;
        }

        public async Task<ResultServices> AddCarBrand(CarBrand entity, IFormFile image)
        {
            if (image == null) return new ResultServices { Succesd = false, Msg = "Invalid imagen " };
            if (entity == null) return new ResultServices { Succesd = false, Msg = "Invalid entity " };
            try
            {
                var resultImage = await fileServices.AddImageAsync("wwwroot/images/CarBrand", image);
                if (!resultImage.Succesd) return new ResultServices { Succesd = false, Msg = resultImage.Msg };
                entity.Image = resultImage.Msg;
                await unitOfWork.Repository<CarBrand>().AddAsync(entity);
                return new ResultServices { Succesd = true, Msg = "CarBrand created" };
            }
            catch (Exception ex)
            {
                await fileServices.DeleteImageAsync("wwwroot/images/CarBrand", entity.Image);
                return new ResultServices { Succesd = false, Msg = ex.Message };
            }
        }

        public async Task<ResultServices> DeleteCarBrand(CarBrand entity)
        {
            if (entity == null) return new ResultServices { Succesd = false, Msg = "Invalid entity " };
            try
            {
                var imageold = entity.Image;
                await unitOfWork.Repository<CarBrand>().DeleteAsync(entity);
                var resultImage = await fileServices.DeleteImageAsync("wwwroot/images/CarBrand", imageold);
                if (!resultImage.Succesd) return new ResultServices { Succesd = false, Msg = resultImage.Msg };
                return new ResultServices { Succesd = true, Msg = "CarBrand delete" };
            }
            catch (Exception ex)
            {
                return new ResultServices { Succesd = false, Msg = ex.Message };
            }
        }

        public IQueryable<CarBrand> GetCarBrand()
        {
            return unitOfWork.Repository<CarBrand>().GetQueryable();
        }

        public Expression<Func<CarBrand, T>> Expression<T>(Func<CarBrand, T> expression)
        {
            return e => expression(e);
        }

        public async Task<CarBrand> GetCarBrandById(string id)
        {
            return await unitOfWork.Repository<CarBrand>().FindOneAsync(x => x.Id == id);
        }

        public async Task<ResultServices> UpdateCarBrand(CarBrand entity, IFormFile? image)
        {
            if (entity == null) return new ResultServices { Succesd = false, Msg = "Invalid entity " };

            try
            {
                if (image != null)
                {
                    var resultImage = await fileServices.UpdateImage(entity.Image, "wwwroot/images/CarBrand", image);
                    if (!resultImage.Succesd) return new ResultServices { Succesd = false, Msg = resultImage.Msg };
                    entity.Image = resultImage.Msg;
                }
                await unitOfWork.Repository<CarBrand>().UpdateAsync(entity);
                return new ResultServices { Succesd = true, Msg = "CarBrand updated" };
            }
            catch (Exception ex)
            {
                return new ResultServices { Succesd = false, Msg = ex.Message };
            }
        }
    }
}