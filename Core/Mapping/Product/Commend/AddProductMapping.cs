using Core.Meditor.Product.Commend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Product
{
    public partial class ProductMapping
    {
        public void AddProductMapping()
        {
            CreateMap<AddProductModelCommend, Domain.Models.ProductListing>()
                 .ForMember(des => des.SKU, src => src.MapFrom(src => src.SKU))
             .ForMember(des => des.SellerID, src => src.MapFrom(src => src.SellerID));
        }
    }
}