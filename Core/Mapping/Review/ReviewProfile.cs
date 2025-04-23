using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Review
{
    public partial  class ReviewProfile:Profile
    {

        public ReviewProfile()
        {
            AddReviewMapping();
            UpdateReviewMapping();
        }

    }
}
