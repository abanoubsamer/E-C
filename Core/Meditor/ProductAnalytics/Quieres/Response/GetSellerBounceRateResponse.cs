using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.ProductAnalytics.Quieres.Response
{
    public class GetSellerOverViewAnalyticsResponse
    {
        public int TotalViews { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalInteractions { get; set; }
        public double interactionRate { get; set; }
    }
}
