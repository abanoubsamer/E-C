using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Commend.Models
{
    public class UpdateUserCardItemsModelCommend:IRequest<Response<string>>
    {
        public string CardItemsId { get; set; }
        public int quantity { get; set; }
    }
}
