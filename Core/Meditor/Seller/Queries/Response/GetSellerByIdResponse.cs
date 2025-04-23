using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Queries.Response
{
    public class GetSellerByIdResponse
    {
       public string id { get; set; }
       public string sellerId { get; set; }
       public string name { get; set; }
       public string email { get; set; }
       public string dateCreate { get; set; }
       public string picture { get; set; }
       public string type { get; set; }
       public string country { get; set; }
       public string ShopName { get; set; }
       public string ContactInfo { get; set; }
 
    }
}
