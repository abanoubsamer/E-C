using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.CarBrand.Queries.Response
{
    public class GetCarBrandResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public GetCarBrandResponse(Domain.Models.CarBrand carBrand)
        {
            Id = carBrand.Id;
            Name = carBrand.Name;
            Image = carBrand.Image;
        }
    }
}