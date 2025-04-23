using Core.Basic;
using Domain.Enums.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Order.Commend.Models
{
    public class UpdateStatusOrderModelCommend:IRequest<Response<string>>
    {
        public required string OrderId { get; set; }
        public required string ProductID { get; set; }
        public required OrderItemStatus Status { get; set; }
    }
}
