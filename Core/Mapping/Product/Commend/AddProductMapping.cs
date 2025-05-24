using Core.Meditor.Product.Commend.Models;
using Domain.Models;
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
            CreateMap<AddProductModelCommend, ProductListing>()
            .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
            .ForMember(dest => dest.SellerID, opt => opt.MapFrom(src => src.SellerID))
            .AfterMap((src, dest) =>
            {
                if (src.modelCompatibilityDtos != null)
                {
                    dest.Product = new ProductMaster
                    {
                        SKU = src.SKU,
                        CategoryID = src.CategoryID,
                        Compatibilities = src.modelCompatibilityDtos.Select(dto => new ModelCompatibility
                        {
                            MaxYear = dto.MaxYear,
                            MinYear = dto.MinYear,
                            ModelId = dto.ModelId,
                            SKU = src.SKU
                        }).ToList()
                    };
                }
               

            });
        }
    }
}