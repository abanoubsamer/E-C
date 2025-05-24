using AutoMapper;
using Core.Basic;
using Core.Meditor.Product.Commend.Models;
using Domain.Models;
using Hangfire;
using MediatR;
using Services.FileSystemServices;
using Services.ProductServices;

namespace Core.Meditor.Product.Commend.Handleing
{
    public class ProductHandlingCommend : ResponseHandler,
        IRequestHandler<AddProductModelCommend, Response<string>>,
        IRequestHandler<UpdateProductModelCommend, Response<string>>,
        IRequestHandler<DeleteProductModelQueries, Response<string>>
    {
        #region Fialds

        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        #endregion Fialds

        #region Constractor

        public ProductHandlingCommend(IProductServices productServices, IMapper mapper)
        {
            _mapper = mapper;
            _productServices = productServices;
        }

        #endregion Constractor

        #region Handling

        public async Task<Response<string>> Handle(AddProductModelCommend request, CancellationToken cancellationToken)
        {
            var ProductMapping = _mapper.Map<Domain.Models.ProductListing>(request);

            if (ProductMapping == null) return BadRequest<string>("Product Invald");

            var result = await _productServices.AddProductAsync(ProductMapping, request.CategoryID, request.MainImage, request.FormImages);

            if (result.Succesd)
            {
                BackgroundJob.Schedule(() =>
                 _productServices.SendWeeklyReminderAndStartRecurring(ProductMapping.ProductID),
                 TimeSpan.FromDays(7));
                return Created(result.Msg);
            }

            return BadRequest<string>(result.Msg);
        }

        public async Task<Response<string>> Handle(UpdateProductModelCommend request, CancellationToken cancellationToken)
        {
            var product = await _productServices.GetProductByID(request.Id);
            if (product == null) return NotFound<string>("Not Found Product");

            product = _mapper.Map(request, product);

            var result = await _productServices.UpdateProductAsync(product, request.MainImages, request.FormImages, request.IdIamgesDelteted);
            if (!result.Succesd) return UnprocessableEntity<string>(result.Msg);

            return Updated<string>("Succesed Update Product");
        }

        public async Task<Response<string>> Handle(DeleteProductModelQueries request, CancellationToken cancellationToken)
        {
            var result = await _productServices.DeleteProductAsync(request.ProductId);
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Deleted<string>("Succed Delete Product");
        }

        #endregion Handling
    }
}