using Core.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.ProductAnalytics.Commend.Models
{
    public class AddProductViewModel:IRequest<Response<string>>
    {
        public string productId {  get; set; } 
        
        public string? userId {  get; set; }

        public DateTime ViewDate { get; set; } = DateTime.Now; 
            
    }
}
