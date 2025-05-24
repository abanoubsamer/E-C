using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Product.Queries.Models
{
    public class AutoCompleteSearchProductModel : IRequest<Response<List<string>>>
    {
        public string SearchText { get; set; }

        public AutoCompleteSearchProductModel(string searchText)
        {
            SearchText = searchText;
        }
    }
}