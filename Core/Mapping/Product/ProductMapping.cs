using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Product
{
    public partial class ProductMapping : Profile
    {
        public  ProductMapping()
        {
            AddProductMapping();
            GetProductList();
            GetProductIdMapping();
            UpdateProductMapping();
        }

    }
}
