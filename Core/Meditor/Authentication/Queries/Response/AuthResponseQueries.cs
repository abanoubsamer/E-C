using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Response
{
    public class AuthResponseQueries
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string Username { get; set; }

        public string UserID { get; set; }

        public List<string> Roles { get; set; }


    }
}
