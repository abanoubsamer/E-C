using Core.Dtos;
using Core.Meditor.Product.Queries.Response;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Product
{
    public partial class ProductMapping
    {
        public void GetProductIdMapping()
        {
            CreateMap<Domain.Models.ProductListing, GetProductByIdResponsesQueries>()
                  .ForMember(des => des.ProductID, src => src.MapFrom(src => src.ProductID.ToString()))
                  .ForPath(des => des.Seller.id, src => src.MapFrom(src => src.Seller.SellerID.ToString()))
                  .ForPath(des => des.Seller.name, src => src.MapFrom(src => src.Seller.User.Name))
              .ForPath(des => des.Category.Id, src => src.MapFrom(src => src.Product.Category.CategoryID.ToString()))
                   .ForPath(des => des.Category.Name, src => src.MapFrom(src => src.Product.Category.Name))

                  .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                      src.Images.Select(img => new ProductImagesDto
                      {
                          id = img.ImageID.ToString(),
                          Image = img.ImageUrl
                      }).ToList()
                  ));
        }
    }
}