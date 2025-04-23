using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Commend.Models
{
    public class DeleteReviewModelCommend:IRequest<Response<string>>
    {

        public string Id { get; set; }
        public DeleteReviewModelCommend(string id)
        {
            Id = id;
        }
    }
}
