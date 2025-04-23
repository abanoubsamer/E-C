using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Commend.Models
{
    public class CancelOrdermodelCommend : IRequest<Response<string>>
    {

        public string Id { get; set; }
        public CancelOrdermodelCommend(string id)
        {
            Id = id;
        }
    }
}
