using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Category
{
    public partial class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            GetCategoryByIdMapping();
            UpdateCategoryMapping();
            AddCategoryMapping();
            GetAllCategorys();
        }
    }
}