using Core.Basic;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Commend.Models
{
    public class UpdateUserModel:IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public IFormFile FormImages { get; set; }

    }
}
