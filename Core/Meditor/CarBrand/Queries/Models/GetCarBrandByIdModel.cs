using Core.Basic;
using Core.Meditor.CarBrand.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.CarBrand.Queries.Models
{
    public class GetCarBrandByIdModel : IRequest<Response<GetCarBrandByIdResponse>>
    {
        public string Id { get; }

        public GetCarBrandByIdModel(string Id)
        {
            this.Id = Id;
        }
    }
}