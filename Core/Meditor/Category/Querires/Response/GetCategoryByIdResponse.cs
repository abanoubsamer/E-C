using Core.Dtos;
using Domain.Dtos;

namespace Core.Meditor.Category.Querires.Response
{
    public class GetCategoryByIdResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public CategoryDto? parantCategory { get; set; }
        public List<CategoryDto>? SubCategory { get; set; }
    }
}