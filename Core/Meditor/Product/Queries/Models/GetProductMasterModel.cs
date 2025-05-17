using Core.Basic;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Queries.Models
{
    public class GetProductMasterModel : IRequest<Response<List<ProductMasterDto>>>
    {
        public string SKU { get; set; }

        public GetProductMasterModel(string sKU)
        {
            SKU = sKU;
        }
    }
}