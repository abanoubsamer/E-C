using Core.Meditor.Model.Queires.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Models
{
    public partial class ModelProfile
    {
        public void GetModelWithBrandMapping()
        {
            CreateMap<Domain.Models.Models, GetModelResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MaxYear, opt => opt.MapFrom(src => src.MaxYear))
                .ForMember(dest => dest.MinYear, opt => opt.MapFrom(src => src.MinYear))
                .ForMember(dest => dest.BrandImage, opt => opt.MapFrom(src => src.Brand.Image))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        }
    }
}