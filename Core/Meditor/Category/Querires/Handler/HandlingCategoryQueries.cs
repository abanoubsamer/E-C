using AutoMapper;
using Core.Basic;
using Core.Meditor.Category.Querires.Models;
using Core.Meditor.Category.Querires.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using Domain.Models;
using MediatR;
using Services.CategoryServices;
using Services.ExtinsionServies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Querires.Handler
{
    public class HandlingCategoryQueries : ResponseHandler,
        IRequestHandler<GetCategoryPagination, PaginationResult<GetCategoryPaginagtionResponseQueries>>,
        IRequestHandler<GetCtegoryByIdModelQueries, Response<GetCategoryByIdResponse>>,
        IRequestHandler<GetParentCategoriesModel, Response<List<GetCategoriesResponse>>>,
        IRequestHandler<GetSubcategoriesModel, Response<List<GetCategoriesResponse>>>
     
    {
        #region Failds
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor
        public HandlingCategoryQueries(ICategoryServices categoryServices, IMapper mapper)
        {
            _mapper = mapper;
            _categoryServices = categoryServices;
        }
        #endregion


        #region Handling
        public async Task<PaginationResult<GetCategoryPaginagtionResponseQueries>> Handle(GetCategoryPagination request, CancellationToken cancellationToken)
        {
            var expression = _categoryServices.CreateExpression(x => new GetCategoryPaginagtionResponseQueries(x));
            var Filter = _categoryServices.FilterCategory(request.CategoryName, request.OrederBy);
            var PaginationList = await Filter.Select(expression).ToPaginationListAsync(request.PageNumber, request.PageSize);
            PaginationList.Meta = new
            {
                Count = PaginationList.Data.Count(),
                Date = DateTime.Now.ToShortDateString()
            };

            return PaginationList;
        }

        public async Task<Response<GetCategoryByIdResponse>> Handle(GetCtegoryByIdModelQueries request, CancellationToken cancellationToken)
        {
            if (request._id.IsNullOrEmpty()) return BadRequest<GetCategoryByIdResponse>("Invalid Id");
            
            var cat = await _categoryServices.GetCategoryById(request._id);
            
            if(cat == null ) return NotFound<GetCategoryByIdResponse>("Not Found Category");

            var CatMapping = _mapper.Map<GetCategoryByIdResponse>(cat);
           

            return Success(CatMapping);
        }

        public async Task<Response<List<GetCategoriesResponse>>> Handle(GetSubcategoriesModel request, CancellationToken cancellationToken)
        {
            var catetegory = await _categoryServices.GetSubcategories(request.Id);
            if (catetegory == null) return NotFound<List<GetCategoriesResponse>>("Not Found Category");

            var mapping = catetegory.Select(x => new GetCategoriesResponse
            {
                Id = x.CategoryID,
                Name = x.Name,
            }).ToList();

            return Success(mapping);
        }

        public async Task<Response<List<GetCategoriesResponse>>> Handle(GetParentCategoriesModel request, CancellationToken cancellationToken)
        {
            var catetegory = await _categoryServices.GetParentCategories();
            if (catetegory == null) return NotFound<List<GetCategoriesResponse>>("Not Found Category");

            var mapping = catetegory.Select(x => new GetCategoriesResponse
            {
                Id = x.CategoryID,
                Name = x.Name,
            }).ToList();

            return Success(mapping);
        }
        #endregion


    }
}
