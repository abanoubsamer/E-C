using Core.Meditor.Category.Commend.Models;
using Services.ExtinsionServies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Category
{
    public partial class CategoryProfile
    {
        public void AddCategoryMapping()
        {
            CreateMap<AddCategoryModelCommend, Domain.Models.Category>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.ParentCategoryID, opt => opt.Condition(src => !src.ParentCategoryID.IsNullOrEmpty()));
        }

    }
}
