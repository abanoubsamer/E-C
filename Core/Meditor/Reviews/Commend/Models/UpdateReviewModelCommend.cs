using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Commend.Models
{
    public class UpdateReviewModelCommend : IRequest<Response<string>>
    {
        [Required]
        public string ReviewId { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
