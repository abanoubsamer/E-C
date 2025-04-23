using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Review.Qureies
{
    public class GetReviewPaginationResponseDto
    {
        public string ReviewID { get; set; }

        public UserDto User { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public string ReviewDate { get; set; }
    }
}
