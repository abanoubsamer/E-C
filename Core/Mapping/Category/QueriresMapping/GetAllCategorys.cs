using Core.Meditor.Category.Querires.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Category
{
    public partial class CategoryProfile
    {
        public void GetAllCategorys()
        {
            CreateMap<Domain.Models.Category, GetCategoryResponseQueries>()
             .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src =>
              src.SubCategories.Select(sub => new Domain.Models.Category
              {
                  Name = sub.Name,
                  CategoryID = sub.CategoryID,
                  Image = sub.Image
              }).ToList()));
        }
    }
}