using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Commend.Models
{
    public class SellerEmialIsExistModel : IRequest<Response<bool>>
    {
        [Required]
        public string Email { get; set; }
    }
}
