using AutoMapper;
using Core.Basic;
using Core.Meditor.Category.Commend.Models;
using Domain.Models;
using MediatR;
using Services.CategoryServices;
using Services.ExtinsionServies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Commend.Handler
{
    public class HandlerCategoryCommend : ResponseHandler,
        IRequestHandler<AddCategoryModelCommend, Response<string>>,
        IRequestHandler<DeleteCtegoryModelQueries, Response<string>>,
        IRequestHandler<UpdateCategoryModelCommend, Response<string>>
    {
        #region Fialds

        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        #endregion Fialds

        #region Constractor

        public HandlerCategoryCommend(ICategoryServices categoryServices, IMapper mapper)
        {
            _mapper = mapper;
            _categoryServices = categoryServices;
        }

        #endregion Constractor

        #region Handler

        public async Task<Response<string>> Handle(AddCategoryModelCommend request, CancellationToken cancellationToken)
        {
            var CategoryMapping = _mapper.Map<Domain.Models.Category>(request);

            var result = await _categoryServices.AddCategory(CategoryMapping, request.formFile);

            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Created("Succed Create Category");
        }

        public async Task<Response<string>> Handle(DeleteCtegoryModelQueries request, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.DeleteCategory(request._id);

            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Deleted<string>("Succed Delete Category");
        }

        public async Task<Response<string>> Handle(UpdateCategoryModelCommend request, CancellationToken cancellationToken)
        {
            var category = await _categoryServices.GetCategoryById(request.Id);

            if (category == null) return NotFound<string>("Not Found Category With Id");

            category = _mapper.Map(request, category);

            var result = await _categoryServices.UpdateCategory(category);

            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Updated<string>("Succed Update Category");
        }

        #endregion Handler
    }
}