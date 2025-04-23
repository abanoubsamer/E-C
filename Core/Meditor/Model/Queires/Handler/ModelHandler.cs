using AutoMapper;
using Core.Basic;
using Core.Meditor.Model.Queires.Models;
using Core.Meditor.Model.Queires.Response;
using MediatR;
using Services.ModelsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Queires.Handler
{
    public class ModelHandler : ResponseHandler,
        IRequestHandler<GetModels, Response<List<GetModelResponse>>>,
        IRequestHandler<GetModelById, Response<GetModelResponse>>
    {
        private readonly IModelsServices modelsServices;
        private readonly IMapper mapper;

        public ModelHandler(IModelsServices modelsServices, IMapper mapper)
        {
            this.modelsServices = modelsServices;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetModelResponse>>> Handle(GetModels request, CancellationToken cancellationToken)
        {
            var models = await modelsServices.GetModelsWithCarBrand(request.BrandId);
            if (models == null) return NotFound<List<GetModelResponse>>();
            var mapping = mapper.Map<List<GetModelResponse>>(models);
            return Success(mapping);
        }

        public async Task<Response<GetModelResponse>> Handle(GetModelById request, CancellationToken cancellationToken)
        {
            var model = await modelsServices.GetModelById(request.Id);
            if (model == null) return NotFound<GetModelResponse>();
            var mapping = mapper.Map<GetModelResponse>(model);
            return Success(mapping);
        }
    }
}