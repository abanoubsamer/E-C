using Core.Basic;
using Core.Meditor.CarBrand.Commend.Models;
using MediatR;
using Services.CarBrandServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.CarBrand.Commend.Hnadler
{
    public class CarBrandCommendHandler : ResponseHandler,
        IRequestHandler<AddCarBrandModel, Response<string>>
    {
        private readonly ICarBrandServices carBrandServices;

        public CarBrandCommendHandler(ICarBrandServices carBrandServices)
        {
            this.carBrandServices = carBrandServices;
        }

        public async Task<Response<string>> Handle(AddCarBrandModel request, CancellationToken cancellationToken)
        {
            var maping = new Domain.Models.CarBrand
            {
                Name = request.Name
            };
            var result = await carBrandServices.AddCarBrand(maping, request.Image);
            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Success("Success");
        }
    }
}