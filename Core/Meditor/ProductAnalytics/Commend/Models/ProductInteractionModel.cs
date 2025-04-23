using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.ProductAnalytics.Commend.Models
{
    public class ProductInteractionModel : IRequest<Response<string>>
    {
        public string ProductId { get; set; }
        public string? UserId { get; set; }
        public string InteractionType { get; set; } = string.Empty;
    }
}
