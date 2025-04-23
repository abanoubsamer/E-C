using Core.Basic;
using Core.Meditor.Product.Queries.Response;
using MediatR;

namespace Core.Meditor.Product.Queries.Models
{
    public class GetProductByIdModelQueries : IRequest<Response<GetProductByIdResponsesQueries>>
    {
        public string ProductId { get; set; }

        public GetProductByIdModelQueries(string productId)
        {
            ProductId = productId;
        }

    }
}
