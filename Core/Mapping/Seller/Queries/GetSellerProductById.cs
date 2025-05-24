using Core.Meditor.Seller.Queries.Response;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;

namespace Core.Mapping.Seller
{
    public partial class SellerProfile
    {
        public void GetSellerProductById()
        {
            CreateMap<Domain.Models.ProductListing, GetSellerProductByIdResponse>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.stock, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.descreption, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ProductImagesDto, opt => opt.MapFrom(src => src.Images.Select(x => new ProductImagesDto
            {
                id = x.ImageID,
                Image = x.ImageUrl
            }).ToList()))
            .ForMember(dest => dest.category, opt => opt.MapFrom(src => new CategoryDto
            {
                Id = src.Product.Category.CategoryID,
                Name = src.Product.Category.Name
            }))
            .ForMember(dest => dest.modelCompatibilityDto, opt => opt.MapFrom(src => src.Product.Compatibilities.Select(x => new GetModelCompatibilitySellerDto
            {
                Id = x.Id,
                BrandName = x.Model.Brand.Name,
                ModelName = x.Model.Name,
                Max = x.MaxYear,
                Min = x.MinYear
            }).ToList()));
        }
    }
}