using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Commend.Models
{
    public class DeleteCardItemModelCommend:IRequest<Response<string>>
    {
        public string Id { get; set; }
        public DeleteCardItemModelCommend(string Id)
        {
            this.Id = Id;
        }

    }
}
