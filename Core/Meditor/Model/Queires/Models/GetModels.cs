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
    public class GetModels : IRequest<Response<List<GetModelResponse>>>
    {
        public string BrandId { get; set; }

        public GetModels(string id)
        {
            BrandId = id;
        }
    }
}