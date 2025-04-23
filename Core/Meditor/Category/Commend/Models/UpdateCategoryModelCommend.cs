using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Category.Commend.Models
{
    public class UpdateCategoryModelCommend : IRequest<Response<string>>
    {
     
        public required string Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public string? ParentCategoryID { get; set; }

    }
}
