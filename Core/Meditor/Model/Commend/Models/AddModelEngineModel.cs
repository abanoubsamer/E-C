using Core.Basic;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Model.Commend.Models
{
    public class AddModelEngineModel : IRequest<Response<string>>
    {
        public string Name { get; set; }

        public string EngineTypeId { get; set; }

        public string ModelId { get; set; }
    }
}