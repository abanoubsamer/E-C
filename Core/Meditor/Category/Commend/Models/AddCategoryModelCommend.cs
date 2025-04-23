using Core.Basic;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Commend.Models
{
    public class AddCategoryModelCommend : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string? ParentCategoryID { get; set; }
        public IFormFile formFile { get; set; }
    }
}