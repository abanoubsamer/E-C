using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Models
{
    public partial class ModelProfile : Profile
    {
        public ModelProfile()
        {
            GetModelWithBrandMapping();
        }
    }
}