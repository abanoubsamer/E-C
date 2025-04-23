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
            CreateMap<AddProductModelCommend, Domain.Models.Product>()

             .ForMember(des => des.CategoryID, src => src.MapFrom(src => src.CategoryID))
             .ForMember(des => des.SellerID, src => src.MapFrom(src => src.SellerID));
        }
    }
}