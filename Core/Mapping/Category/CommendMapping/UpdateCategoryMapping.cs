using Core.Meditor.Category.Commend.Models;
using Domain.Models;
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

        public void UpdateCategoryMapping()
        {
            CreateMap<UpdateCategoryModelCommend, Domain.Models.Category>()
                .ForMember(des => des.CategoryID, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.Name, opt => opt.Condition(src => !src.Name.IsNullOrEmpty()))
                .ForMember(des => des.ParentCategoryID, opt => opt.Condition(src => !src.ParentCategoryID.IsNullOrEmpty()));
        }

    }
}
