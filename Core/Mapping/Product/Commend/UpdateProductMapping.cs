using Core.Meditor.Product.Commend.Models;
using Microsoft.IdentityModel.Tokens;
using Services.ExtinsionServies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Product
{
    public partial class ProductMapping
    {
        public void UpdateProductMapping()
        {
            CreateMap<UpdateProductModelCommend, Domain.Models.ProductListing>()
               .ForMember(des => des.ProductID, src => src.MapFrom(src => src.Id))
               .ForMember(dest => dest.StockQuantity, opt => opt.Condition(src => src.StockQuantity > 0))
               .ForMember(dest => dest.Name, opt => opt.Condition(src => !src.Name.IsNullOrEmpty()))
               .ForMember(dest => dest.Description, opt => opt.Condition(src => !src.Description.IsNullOrEmpty()))
               .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price > 0))
               // .ForMember(dest => dest.Product.CategoryID, opt => opt.Condition(src => !src.CategoryID.IsNullOrEmpty()))
               .ForMember(dest => dest.SellerID, opt => opt.Condition(src => !src.SellerID.IsNullOrEmpty()));
        }
    }
}