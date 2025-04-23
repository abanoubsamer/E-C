using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Result
{
    public class AuthModelResult
    {

        public string Messgage { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }


        public bool IsAuthenticated { get; set; }

        public DateTime Expiration { get; set; }


        public List<string> Roles { get; set; }


        public string? RefreshToken { get; set; }


        public DateTime RefreshTokenExpiration { get; set; }

    }
}
