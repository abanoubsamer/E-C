using AutoMapper;
using Core.Basic;
using Core.Meditor.Authentication.Queries.Model;
using Core.Meditor.User.Queries.Model;
using Core.Meditor.User.Queries.Response;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Services.AuthenticationServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Handling
{
    public class AuthenticationHandlingQueries : ResponseHandler
        , IRequestHandler<LoginModelQueries, Response<AuthResponseQueries>>
        , IRequestHandler<GoogleAuthLoginModel, Response<AuthResponseQueries>>
        , IRequestHandler<LoginSellerModel, Response<AuthResponseQueries>>
        , IRequestHandler<EmailIsExistModelQueries, Response<bool>>
        , IRequestHandler<RefrashTokenModelQueries, Response<AuthResponseQueries>>
    {
        #region Fields

        private readonly IAuthenticationServices _authorizationService;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Fields

        #region Constructor

        public AuthenticationHandlingQueries(
            SignInManager<ApplicationUser> signInManager,
           IAuthenticationServices authenticationServices,
           IMapper mapper,
           IUserServices userServices,
           IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _authorizationService = authenticationServices;
            _mapper = mapper;
            _userServices = userServices;

            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Constructor

        public async Task<Response<AuthResponseQueries>> Handle(LoginModelQueries request, CancellationToken cancellationToken)
        {
            var resultCheck = await _authorizationService.CheckEmail(request.Email, request.Password);

            if (!resultCheck.Succesd) return BadRequest<AuthResponseQueries>(resultCheck.Msg);

            var AuthModel = await _authorizationService.GetTokenAsync(resultCheck.User);

            if (!AuthModel.IsAuthenticated) return Unauthorized<AuthResponseQueries>(AuthModel.Messgage);

            SetInCookies("RefreshToken", AuthModel.RefreshToken, AuthModel.RefreshTokenExpiration);

            var AuthMapping = _mapper.Map<AuthResponseQueries>(AuthModel);

            return Success(AuthMapping);
        }

        public async Task<Response<AuthResponseQueries>> Handle(RefrashTokenModelQueries request, CancellationToken cancellationToken)
        {
            var AuthModel = await _authorizationService.RefreshToken(request.RefreshToken, request.AccessToken);

            if (!AuthModel.IsAuthenticated) return Unauthorized<AuthResponseQueries>(AuthModel.Messgage);

            var AuthMapping = _mapper.Map<AuthResponseQueries>(AuthModel);

            return Success(AuthMapping);
        }

        public async Task<Response<AuthResponseQueries>> Handle(GoogleAuthLoginModel request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                Id = request.UserID,
                EmailConfirmed = true,
                UserName = request.Email,
                Picture = request.Picture,
                Name = request.Name,
            };
            var AuthModel = await _authorizationService.LoginWithGoogle(user);

            if (!AuthModel.IsAuthenticated) BadRequest<AuthResponseQueries>(AuthModel.Messgage);

            SetInCookies("RefreshToken", AuthModel.RefreshToken, AuthModel.RefreshTokenExpiration);
            SetInCookies("AuthToken", AuthModel.Token, AuthModel.Expiration);
            SetInCookies("UserId", AuthModel.UserId, AuthModel.Expiration);
            var AuthMapping = _mapper.Map<AuthResponseQueries>(AuthModel);

            return Success(AuthMapping);
        }

        public async Task<Response<bool>> Handle(EmailIsExistModelQueries request, CancellationToken cancellationToken)
        {
            return await _userServices.EmailIsExist(request._email) ? Success(true) : Success(false);
        }

        public async Task<Response<AuthResponseQueries>> Handle(LoginSellerModel request, CancellationToken cancellationToken)
        {
            var resultCheck = await _authorizationService.CheckEmailSeller(request.Email, request.Password);

            if (!resultCheck.Succesd) return BadRequest<AuthResponseQueries>(resultCheck.Msg);

            //if (!resultCheck.User.EmailConfirmed) return Unauthorized<AuthResponseQueries>("Email Not Confirmed");

            var AuthModel = await _authorizationService.GetTokenAsync(resultCheck.User);

            if (!AuthModel.IsAuthenticated) return Unauthorized<AuthResponseQueries>(AuthModel.Messgage);

            SetInCookies("RefreshToken", AuthModel.RefreshToken, AuthModel.RefreshTokenExpiration);

            var AuthMapping = _mapper.Map<AuthResponseQueries>(AuthModel);

            return Success(AuthMapping);
        }

        private void SetInCookies(string Name, string Token, DateTime Expires)
        {
            var CookiesOtion = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = Expires.ToLocalTime(),
                SameSite = SameSiteMode.None
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(Name, Token, CookiesOtion);
        }
    }
}