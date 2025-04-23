using AutoMapper;
using Core.Basic;
using Core.Meditor.Authentication.Commend.Model;
using Core.Meditor.User.Commend.Model;
using Core.Meditor.User.Queries.Response;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Services.AuthenticationServices;
using Services.SellerServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Commend.Hnadling
{
    public class AuthenticationHandlingCommend : ResponseHandler,
        IRequestHandler<RegistrationUserModelCommend, Response<string>>,
        IRequestHandler<ValidationTokenCommend, Response<string>>,
        IRequestHandler<RegistrationSellerModelCommend, Response<string>>
    {

        #region Fialds
        private readonly IAuthenticationServices _authServices;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor
        public AuthenticationHandlingCommend(IAuthenticationServices authServices, IMapper mapper)
        {
            _mapper = mapper;
            _authServices =authServices;
        }

        #endregion


        #region Handling
        public async Task<Response<string>> Handle(RegistrationUserModelCommend request, CancellationToken cancellationToken)
        {
            var UserMapping = _mapper.Map<ApplicationUser>(request);

            var result = await _authServices.Registration(UserMapping, request.Password,"User");
           
            if (!result.Succesd) return UnprocessableEntity<string>(result.Msg);

            
            return Created<string>("Succesed Reqistraction");


        }

        public async Task<Response<string>> Handle(RegistrationSellerModelCommend request, CancellationToken cancellationToken)
        {
            var sellerMapping = _mapper.Map<ApplicationUser>(request);
            var result = await _authServices.Registration(sellerMapping, request.Password,"Seller");
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            
            return Created("Secces Create");
        }

        public async Task<Response<string>> Handle(ValidationTokenCommend request, CancellationToken cancellationToken)
        {
            var claims = _authServices.ValidationToken(request.Token);
            if (claims == null) return  BadRequest<string>("Invalid Token");
            return Success("Token IS Falid");
        }


        #endregion

    }
}
