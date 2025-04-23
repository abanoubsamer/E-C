using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Commend.Models
{
    public class DeleteProductModelQueries:IRequest<Response<string>>
    {
        public string ProductId { get; set; }

        public DeleteProductModelQueries(string productId)
        {
            ProductId = productId;
        }
    }
}
