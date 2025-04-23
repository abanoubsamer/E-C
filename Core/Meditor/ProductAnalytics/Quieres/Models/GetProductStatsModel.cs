using Core.Basic;
using Core.Meditor.ProductAnalytics.Quieres.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.ProductAnalytics.Quieres.Models
{
    public class GetProductStatsModel:IRequest<Response<GetProductStatsResponse>>
    {
        public string ProductId {  get; set; }
        public GetProductStatsModel(string productId)
        {
            ProductId = productId;
        }
    }
}
