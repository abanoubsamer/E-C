using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Commend.Models
{
    public class AddCardItemModelCommend:IRequest<Response<string>>
    {
        [Required]
        public string ProductID { get; set; }

        [Required]
        public string UserId { get; set; }
       
        [Required]
        public int Quantity { get; set; }
    }
}
