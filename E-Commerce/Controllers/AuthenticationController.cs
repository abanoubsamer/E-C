using Core.Basic;
using Core.Meditor.Authentication.Commend.Model;
using Core.Meditor.Authentication.Queries.Model;
using Core.Meditor.Product.Queries.Models;
using Core.Meditor.User.Commend.Model;
using Core.Meditor.User.Queries.Model;
using Couerses.Basic;
using Domain.MetaData;
using MediatR;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace E_Commerce.Controllers
{
 
    [ApiController]
    public class AuthenticationController : BasicController
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IMediator mediator, IConfiguration configuration) : base(mediator) {
            _configuration = configuration;
        }

        [HttpGet]
        [Route(Routing.Authentication.RefreshToken)]
        public async Task<IActionResult> Refresh()
        {
            var RefreshToekn = Request.Cookies["RefreshToken"];

            var accessToken = Request.Headers["Authorization"].ToString();
            if (accessToken.StartsWith("Bearer "))
            {
                accessToken = accessToken.Substring("Bearer ".Length).Trim();
            }

            if (RefreshToekn == null || accessToken == null)
            {
                var resp = new Response<string>();
                resp.StatusCode = System.Net.HttpStatusCode.NotFound;
                resp.Message = "Not Found";
                return NewResult(resp);
            }
            return NewResult(await _Mediator.Send(new RefrashTokenModelQueries(RefreshToekn,accessToken)));
        }

        [HttpPost]
        [Route(Routing.Authentication.RegisterUser)]
        public async Task<IActionResult> RegisterUser(RegistrationUserModelCommend entity)
        {
            return NewResult(await _Mediator.Send(entity));
        }


        [HttpGet]
        [Route(Routing.Authentication.EmailExist)]
        public async Task<IActionResult> EmailIsExist(string Email)
        {
            return NewResult(await _Mediator.Send(new EmailIsExistModelQueries(Email)));
        }

        [HttpPost]
        [Route(Routing.Authentication.RegisterSeller)]
        public async Task<IActionResult> RegisterSeller(RegistrationSellerModelCommend entity)
        {
            return NewResult(await _Mediator.Send(entity));
        }

        [HttpPost]
        [Route(Routing.Authentication.Login)]
        public async Task<IActionResult> ConfirmEmail(LoginModelQueries Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }


        [HttpPost]
        [Route(Routing.Authentication.LoginSeller)]
        public async Task<IActionResult> LoginSeller(LoginSellerModel Model)
        {
            return NewResult(await _Mediator.Send(Model));
        }


        [HttpGet(Routing.Authentication.LoginWihtGoogle)]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse")};
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
      
        [HttpGet(Routing.Authentication.AuthCallBackGoogle)]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed.");
          
            if (authenticateResult.Principal == null)
                return BadRequest("Google Principal failed.");

            var response = await _Mediator.Send(new GoogleAuthLoginModel(authenticateResult.Principal));

            return Content("<script>window.close();</script>", "text/html");
        }


        [HttpGet(Routing.Authentication.GetToken)]
        public IActionResult GetAuth()
        {
            if (Request.Cookies.TryGetValue("AuthToken", out string token)&& Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return Ok(new { token, userId });
            }

            return Unauthorized("No token found");
        }

        [HttpGet(Routing.Authentication.GetRefreshToken)]
        public IActionResult GetRefreshToken()
        {
            if (Request.Cookies.TryGetValue("RefreshToken", out string token))
            {
                return Ok(new { token});
            }

            return Unauthorized("No token found");
        }

        [HttpPost(Routing.Authentication.ValidationToken)]
        public async Task<IActionResult>  ValidationToken(ValidationTokenCommend model)
        {
            return NewResult(await _Mediator.Send(model));

        }




    }
}
