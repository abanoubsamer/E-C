using Core.Basic;
using Core.Meditor.Model.Commend.Models;
using MediatR;
using Services.ModelCompatibilityServices;
using Services.ModelsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Commend.Handler
{
    public class HandlerModel : ResponseHandler,
        IRequestHandler<AddModelCompatibilityModel, Response<string>>,
        IRequestHandler<AddModel, Response<string>>
    {
        private readonly IModelCompatibilityServices _modelCompatibilityServices;
        private readonly IModelsServices _modelsServices;

        public HandlerModel(IModelCompatibilityServices modelCompatibilityServices, IModelsServices modelsServices)
        {
            _modelsServices = modelsServices;
            _modelCompatibilityServices = modelCompatibilityServices;
        }

        public async Task<Response<string>> Handle(AddModelCompatibilityModel request, CancellationToken cancellationToken)
        {
            var mapping = request.modelCompatibilityDtos.Select(x => new Domain.Models.ModelCompatibility
            {
                MaxYear = x.MaxYear,
                ModelId = x.ModelId,
                ProductId = x.ProductId,
                MinYear = x.MinYear
            }).ToList();

            var result = await _modelCompatibilityServices.AddModelCompatibilityRangeAsync(mapping);

            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Success("Model Compatibility Added");
        }

        public async Task<Response<string>> Handle(AddModel request, CancellationToken cancellationToken)
        {
            var model = new Domain.Models.Models()
            {
                Name = request.Name,
                BrandId = request.BrandId,
                MaxYear = request.MaxYear,
                MinYear = request.MinYear
            };
            var result = await _modelsServices.AddModel(model, request.Image);

            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Success("Model Added");
        }
    }
}