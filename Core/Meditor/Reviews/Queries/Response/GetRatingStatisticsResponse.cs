using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Queries.Response
{
    public class GetRatingStatisticsResponse
    {
        public double AverageRating { get; set; }
        public int NamberReviews { get; set; }
        public Dictionary<int, double> Percentages { get; set; }
    }
}
