using Core.Basic;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Commend.Models
{
    public class AddReviewModelCommend:IRequest<Response<string>>
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        public string ProductID { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

    }
}
