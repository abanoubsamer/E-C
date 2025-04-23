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
    public class GetSellerOverViewAnalytics: IRequest<Response<GetSellerOverViewAnalyticsResponse>>
    {
        public string SellerId { get; set; }
        public GetSellerOverViewAnalytics(string sellerId)
        {
            SellerId = sellerId;
        }
    }
}
