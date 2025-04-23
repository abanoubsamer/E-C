using Domain.Models;
using Microsoft.AspNetCore.Http;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ModelsServices
{
    public interface IModelsServices
    {
        public Task<ResultServices> AddModel(Models entity, IFormFile image);

        public Task<ResultServices> RemoveModel(Models entity);

        public Task<ResultServices> UpdateModel(Models entity);

        public Task<ResultServices> AddModelRange(List<Models> entity);

        public Task<ResultServices> RemoveModelRange(List<Models> entity);

        public Task<Models> GetModelById(string id);

        public Task<List<Models>> GetModelsWithCarBrand(string CarBrandId);
    }
}