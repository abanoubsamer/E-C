using Core.Meditor.Product.Queries.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Meditor.Category.Querires.Response
{
    public class GetCategoryPaginagtionResponseQueries
    {
        public string Name { get; set; }
        public string CategoryID { get; set; }
        public string image { get; set; }

        public List<Domain.Models.Category> SubCategories { get; set; }

        public GetCategoryPaginagtionResponseQueries(Domain.Models.Category Category)
        {
            Name = Category.Name;
            CategoryID = Category.CategoryID;
            image = Category.Image;
            SubCategories = Category.SubCategories
                .Select(x =>
                new Domain.Models.Category
                {
                    Name = x.Name,
                    CategoryID = x.CategoryID,
                    Image = x.Image
                }).ToList();
        }
    }
}