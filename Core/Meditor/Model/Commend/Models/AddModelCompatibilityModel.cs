using Core.Basic;
using Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Commend.Models
{
    public class AddModelCompatibilityModel : IRequest<Response<string>>
    {
        public List<ModelCompatibilityDto> modelCompatibilityDtos { get; set; }
    }
}