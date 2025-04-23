using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Result
{
    public class LoginResult: ResultServices
    {

        public ApplicationUser User { get; set; }

    }
}
