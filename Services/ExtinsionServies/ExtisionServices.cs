using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExtinsionServies
{
    public static class ExtisionServices
    {
        public static bool IsNullOrEmpty(this string entity)
        {
            if (entity == null || entity == string.Empty)
             return  true;
            return false;
        }
   }
}
