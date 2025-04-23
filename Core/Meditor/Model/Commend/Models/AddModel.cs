using Core.Basic;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Commend.Models
{
    public class AddModel : IRequest<Response<string>>
    {
        public string Name { get; set; }

        public int MinYear { get; set; }

        public int MaxYear { get; set; }

        public IFormFile Image { get; set; }

        public string BrandId { get; set; }
    }
}