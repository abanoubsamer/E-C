using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Commend.Models
{
    public class AddUserPhonesModel:IRequest<Response<string>>
    {
        [Required]
        public string UserId { get; set; }
        [Phone]
        [Required]
        public string Phone { get; set; }
    }
}
