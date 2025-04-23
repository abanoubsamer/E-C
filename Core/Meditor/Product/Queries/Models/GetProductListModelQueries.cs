using Core.Basic;
using Core.Meditor.Product.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Queries.Models
{
    public class GetProductListModelQueries : IRequest<Response<List<GetProductListResponseQueries>>>
    {
      
    }
}
