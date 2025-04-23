using Core.Dtos;
using Core.Meditor.Category.Querires.Response;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Category
{
    public partial class CategoryProfile
    {
        public void GetCategoryByIdMapping()
        {
            CreateMap<Domain.Models.Category, GetCategoryByIdResponse>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(des => des.SubCategory, opt => opt.MapFrom(src =>
                 src.SubCategories.Select(x =>
                 new
                 {
                     Name = x.Name,
                     Id = x.CategoryID,
                 })))
                .ForMember(des => des.parantCategory, opt => opt.MapFrom(src => new CategoryDto
                {
                    Id = src.ParentCategory.CategoryID,
                    Name = src.ParentCategory.Name,
                }));
        }
    }
}