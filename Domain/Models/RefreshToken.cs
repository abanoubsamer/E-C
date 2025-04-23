using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Owned]
    public class RefreshToken
    {
        public string ApplicationUserId { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public string AccessToken { get; set; }

        public DateTime ExpirsOn { get; set; }

        public DateTime CreateOn { get; set; }

        public DateTime? RevokeOn { get; set; }

        public bool IsActive => !IsExpired && RevokeOn == null;
        public bool IsExpired => DateTime.Now >= ExpirsOn;

    }
}
