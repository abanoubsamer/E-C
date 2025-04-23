using Core.Basic;
using Domain.OptionsConfiguration;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Mail.Commend.Models
{
    public class SendEmailOtp:IRequest<Response<string>>
    {
        [Required]
        public string Email {  get; set; }
    }
}
