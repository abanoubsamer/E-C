using Core.Basic;
using Core.Meditor.Category.Querires.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Querires.Models
{
    public class GetSubcategoriesModel:IRequest<Response<List<GetCategoriesResponse>>>
    {
        public string Id { get; set; }
        public GetSubcategoriesModel(string id)
        {
            Id = id;
        }

    }
}
