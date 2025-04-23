using Core.Basic;
using Core.Meditor.User.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Queries.Model
{
    public class GoogleAuthLoginModel : IRequest<Response<AuthResponseQueries>>
    {
        public ClaimsPrincipal _claimsPrincipal { get; set; }
        public GoogleAuthLoginModel(ClaimsPrincipal claimsPrincipal)
        {
            UserID = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            Name = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            Picture = claimsPrincipal.FindFirstValue("urn:google:picture");
        }
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

    }
}
