using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ReviewDto
    {
        public string ReviewID { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public string ReviewDate { get; set; }

    }
}
