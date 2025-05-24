using Core.Basic;
using Core.Meditor.Product.Queries.Response;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Meditor.Category.Querires.Response
{
    public class GetCategoryResponseQueries
    {
        public string Name { get; set; }
        public string CategoryID { get; set; }
        public string image { get; set; }
        public List<Domain.Models.Category> SubCategories { get; set; }
    }
}