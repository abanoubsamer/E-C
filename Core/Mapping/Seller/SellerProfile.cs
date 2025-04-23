using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Seller
{
    public partial class SellerProfile:Profile
    {
        public SellerProfile()
        {
            GetSellerById();
        }
    }
}
