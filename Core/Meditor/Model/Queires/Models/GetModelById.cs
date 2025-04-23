using Core.Basic;
using Core.Meditor.Model.Queires.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Queires.Models
{
    public class GetModelById : IRequest<Response<GetModelResponse>>
    {
        public string Id { get; set; }

        public GetModelById(string Id)
        {
            this.Id = Id;
        }
    }
}