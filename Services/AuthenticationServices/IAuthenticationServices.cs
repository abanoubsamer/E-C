using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationServices
{
    public interface IAuthenticationServices
    {
        public Task<AuthModelResult> GetTokenAsync(ApplicationUser user);
        public Task<AuthModelResult> RefreshToken(string RefreshToken, string AccessToken);
        public  Task<LoginResult> CheckEmail(string email, string password);
        public Task<LoginResult> CheckEmailSeller(string email, string password);
        public Task<ResultServices> Registration(ApplicationUser user, string password,string Role);
        public Task<AuthModelResult> LoginWithGoogle(ApplicationUser user);
        public ClaimsPrincipal ValidationToken(string token); 

    }
}
