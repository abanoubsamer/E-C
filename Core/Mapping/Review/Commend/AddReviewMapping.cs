using Azure.Core;
using Core.Meditor.Reviews.Commend.Models;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Review
{
    public partial class ReviewProfile
    {
        
        public void AddReviewMapping()
        {
     
            CreateMap<AddReviewModelCommend, Domain.Models.Review>()
                .ForMember(des => des.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(des => des.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(des => des.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(des => des.UserID, opt => opt.MapFrom(src => src.UserID));
                

        }

    }
}
