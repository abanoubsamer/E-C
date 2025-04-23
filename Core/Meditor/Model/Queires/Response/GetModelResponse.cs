using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Queires.Response
{
    public class GetModelResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public string Image { get; set; }
        public string BrandImage { get; set; }
    }
}