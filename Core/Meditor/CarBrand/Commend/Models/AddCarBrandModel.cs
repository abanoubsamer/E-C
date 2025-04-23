using Core.Basic;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.CarBrand.Commend.Models
{
    public class AddCarBrandModel : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}