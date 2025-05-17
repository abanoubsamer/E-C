using Domain.Models;
using Domain.OptionsConfiguration;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.ExtinsionServies;
using Services.Result;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.AuthenticationServices
{
    public class AuthenticationServices : IAuthenticationServices
    {
        #region Fildes

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IOptions<JwtOptions> _jwtOptions;

        #endregion Fildes

        #region Constractor

        public AuthenticationServices(
           RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> UserManager
         , IOptions<JwtOptions> jwtOptions
         , IUnitOfWork unitOfWork
         )
        {
            _roleManager = roleManager;
            _UnitOfWork = unitOfWork;
            _userManager = UserManager;
            _jwtOptions = jwtOptions;
        }

        #endregion Constractor

        #region Implemntation

        public async Task<AuthModelResult> GetTokenAsync(ApplicationUser user)
        {
            var authModel = new AuthModelResult();

            if (user == null) return authModel;

            var token = await GenrateJwtAsync(user);

            var Roles = await _userManager.GetRolesAsync(user);

            authModel.UserId = user.Id;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authModel.Email = user.Email;
            authModel.SellerId = user.SellerID;
            authModel.UserName = user.UserName;
            authModel.Expiration = token.ValidTo;
            authModel.Roles = Roles.ToList();
            authModel.IsAuthenticated = true;

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var ActiveRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = ActiveRefreshToken.Token;
                authModel.RefreshTokenExpiration = ActiveRefreshToken.ExpirsOn;
            }
            else
            {
                var NewRefrshToken = await GenerateRefreshToken();
                NewRefrshToken.JwtId = token.Id;
                NewRefrshToken.AccessToken = authModel.Token;
                authModel.RefreshToken = NewRefrshToken.Token;
                authModel.RefreshTokenExpiration = NewRefrshToken.ExpirsOn;
                user.RefreshTokens.Add(NewRefrshToken);
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return authModel;
        }

        private async Task<SecurityToken> GenrateJwtAsync(ApplicationUser user)
        {
            //Aggregation User Claims
            var Claims = await GetUserClaimsAsync(user);
            // Get Role Claims
            var RoleClaims = await GetRoleCalimsAsync(user);

            // Aggragation All Claims
            Claims.AddRange(RoleClaims);

            //CreateJwtToken
            var token = CreateJwtToken(Claims);

            return token;
        }

        private SecurityToken CreateJwtToken(List<Claim> Claims)
        {
            //Genrate Key
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
            //Genratate Descriptor Token
            var TokenDesc = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Value.Audience,
                Issuer = _jwtOptions.Value.Issuer,
                Expires = DateTime.UtcNow.AddHours(_jwtOptions.Value.LiveTimeHouer),
                Subject = new ClaimsIdentity(Claims),
                SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            };

            var TokenHanlder = new JwtSecurityTokenHandler();
            return TokenHanlder.CreateToken(TokenDesc);
        }

        private async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            // Get User Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            // new jwt id
            var JWTID = Guid.NewGuid().ToString();
            //Add New Claims
            var NewClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, JWTID)
            };

            if (!string.IsNullOrEmpty(user.SellerID))
            {
                NewClaims.Add(new Claim("SellerId", user.SellerID));
            }

            NewClaims.AddRange(userClaims);

            return NewClaims;
        }

        private async Task<List<Claim>> GetRoleCalimsAsync(ApplicationUser user)
        {
            //Get Role User
            var userRole = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();

            foreach (var roleName in userRole)
            {
                RoleClaims.Add(new Claim(ClaimTypes.Role, roleName));
                var Role = await _roleManager.FindByNameAsync(roleName);
                if (Role != null)
                {
                    var ClaimsForRole = await _roleManager.GetClaimsAsync(Role);
                    RoleClaims.AddRange(ClaimsForRole);
                }
            }

            return RoleClaims;
        }

        public async Task<AuthModelResult> RefreshToken(string RefreshToken, string AccessToken)
        {
            var AuthModel = new AuthModelResult();
            var claims = validationJwtWithOutExpiration(AccessToken);

            if (claims == null)
            {
                AuthModel.Messgage = "Invalid Token";
                return AuthModel;
            }

            //get user with has IS Token
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshTokens.Any(x => x.Token == RefreshToken));

            if (user == null)
            {
                AuthModel.Messgage = "Not Found Token In This User";
                return AuthModel;
            }

            // get Tokens With User

            var storgeRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == RefreshToken);

            var tokenUserId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (tokenUserId.Value != storgeRefreshToken.ApplicationUserId)
            {
                AuthModel.Messgage = "Invalid Token User";
                return AuthModel;
            }

            if (storgeRefreshToken == null || !storgeRefreshToken.IsActive)
            {
                AuthModel.Messgage = "Invalid Token Or Expired";
                return AuthModel;
            }

            //Genertate New Jwt

            var Roles = await _userManager.GetRolesAsync(user);

            var Token = await GenrateJwtAsync(user);

            AuthModel.Token = new JwtSecurityTokenHandler().WriteToken(Token);
            AuthModel.Expiration = Token.ValidTo;
            AuthModel.RefreshToken = storgeRefreshToken.Token;
            AuthModel.RefreshTokenExpiration = storgeRefreshToken.ExpirsOn;
            AuthModel.IsAuthenticated = true;
            AuthModel.UserId = user.Id;
            AuthModel.Email = user.Email;
            AuthModel.UserName = user.UserName;
            AuthModel.Roles = Roles.ToList();
            return AuthModel;
        }

        private async Task<RefreshToken> GenerateRefreshToken()
        {
            var RendemNumber = new byte[32];
            using var Generate = new RNGCryptoServiceProvider();
            Generate.GetBytes(RendemNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RendemNumber),
                CreateOn = DateTime.UtcNow,
                ExpirsOn = DateTime.UtcNow.AddDays(5)
            };
        }

        private ClaimsPrincipal validationJwtWithOutExpiration(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                        ,
                        ValidAudience = _jwtOptions.Value.Audience,
                        ValidIssuer = _jwtOptions.Value.Issuer
                    }, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResultServices> Registration(ApplicationUser user, string password, string Role)
        {
            if (user == null || password.IsNullOrEmpty()) return new ResultServices { Msg = "Invalid Pasword Or Email" };

            var trasnaction = await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description} :";
                    }

                    await _UnitOfWork.RollBackAsync();
                    return new ResultServices { Msg = errors };
                }
                var roleresult = await _userManager.AddToRoleAsync(user, Role);
                if (!roleresult.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description} :";
                    }
                    await _UnitOfWork.RollBackAsync();
                    return new ResultServices { Msg = errors };
                }

                await _UnitOfWork.Repository<Domain.Models.Card>()
                         .AddAsync(new Domain.Models.Card { UserID = user.Id });

                await _UnitOfWork.CommentAsync();
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollBackAsync();
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<LoginResult> CheckEmail(string email, string password)
        {
            if (email.IsNullOrEmpty() || password.IsNullOrEmpty()) return new LoginResult { Msg = "Email Or Password Invalid" };

            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return new LoginResult { Msg = "Email Or Password Invalid" };

                var result = await _userManager.CheckPasswordAsync(user, password);

                if (!result) return new LoginResult { Msg = "Email Or Password Invalid" };

                return new LoginResult { Succesd = true, User = user };
            }
            catch (Exception ex)
            {
                return new LoginResult { Msg = ex.Message };
            }
        }

        public async Task<LoginResult> CheckEmailSeller(string email, string password)
        {
            if (email.IsNullOrEmpty() || password.IsNullOrEmpty()) return new LoginResult { Msg = "Email Or Password Invalid" };

            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null) return new LoginResult { Msg = "Email Or Password Invalid" };

                if (user.Seller == null) return new LoginResult { Msg = "Email Or Password Invalid" };

                var result = await _userManager.CheckPasswordAsync(user, password);

                if (!result) return new LoginResult { Msg = "Email Or Password Invalid" };

                return new LoginResult { Succesd = true, User = user };
            }
            catch (Exception ex)
            {
                return new LoginResult { Msg = ex.Message };
            }
        }

        public async Task<AuthModelResult> LoginWithGoogle(ApplicationUser user)
        {
            if (user == null) return new AuthModelResult { Messgage = " Invalid User" };

            try
            {
                var UserExist = await _userManager.FindByEmailAsync(user.Email);
                if (UserExist == null)
                {
                    var BeginTransaction = await _UnitOfWork.BeginTransactionAsync();
                    // create
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Empty;
                        foreach (var item in result.Errors)
                        {
                            errors += $"{item.Description} :";
                        }
                        return new AuthModelResult { Messgage = errors };
                    }
                    // add role
                    var resultRole = await _userManager.AddToRoleAsync(user, "User");
                    if (!resultRole.Succeeded)
                    {
                        var errors = string.Empty;
                        foreach (var item in result.Errors)
                        {
                            errors += $"{item.Description} :";
                        }
                        return new AuthModelResult { Messgage = errors };
                    }
                    //comment
                    await _UnitOfWork.CommentAsync();
                    //Get Token
                    return await GetTokenAsync(user);
                }

                //Get Token
                return await GetTokenAsync(UserExist);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollBackAsync();
                return new AuthModelResult { Messgage = ex.Message };
            }
        }

        public ClaimsPrincipal ValidationToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidAudience = _jwtOptions.Value.Audience,
                        ValidIssuer = _jwtOptions.Value.Issuer
                    }, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion Implemntation
    }
}