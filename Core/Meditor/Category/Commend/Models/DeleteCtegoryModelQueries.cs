using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Commend.Models
{
    public class DeleteCtegoryModelQueries:IRequest<Response<string>>
    {
        public string _id { get; set; }
        public DeleteCtegoryModelQueries(string id)
        {
            _id = id;
        }

    }
}
