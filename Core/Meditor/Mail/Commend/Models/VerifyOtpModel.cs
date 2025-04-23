using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Mail.Commend.Models
{
    public class VerifyOtpModel:IRequest<Response<string>>
    {

        [Required]
        public int otp {  get; set; }
        [Required]
        public string Email { get; set; }

    }
}
