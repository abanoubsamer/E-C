using Domain.Models;
using Microsoft.AspNetCore.Http;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.CarBrandServices
{
    public interface ICarBrandServices
    {
        public Task<ResultServices> AddCarBrand(CarBrand entity, IFormFile image);

        public Expression<Func<CarBrand, T>> Expression<T>(Func<CarBrand, T> expression);

        public IQueryable<CarBrand> GetCarBrand();

        public Task<CarBrand> GetCarBrandById(string id);

        public Task<ResultServices> UpdateCarBrand(CarBrand entity, IFormFile? image);

        public Task<ResultServices> DeleteCarBrand(CarBrand entity);
    }
}