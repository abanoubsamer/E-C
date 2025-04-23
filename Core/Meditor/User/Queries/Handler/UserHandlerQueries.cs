using AutoMapper;
using Core.Basic;
using Core.Meditor.User.Queries.Modles;
using Core.Meditor.User.Queries.Response;
using Core.Pagination;
using Core.Pagination.Extensions;
using MediatR;
using Services.ExtinsionServies;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Queries.Handler
{
    public class UserHandlerQueries : ResponseHandler,
        IRequestHandler<GetUserPaginationModel, PaginationResult<GetUserPaginationReponse>>,
        IRequestHandler<GetSippingAddressUserModels, Response<List<GetSippingAddressUserResponse>>>,
        IRequestHandler<GetUserByIdModel, Response<GetUserByIdReponse>>
    {

        #region Fialds
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        #endregion


        #region Constractor
        public UserHandlerQueries(IUserServices userServices, IMapper mapper)
        {
            _mapper = mapper;   
            _userServices = userServices;
        }
        #endregion


        #region Handler
        public async Task<PaginationResult<GetUserPaginationReponse>> Handle(GetUserPaginationModel request, CancellationToken cancellationToken)
        {
            //create Expretion
            var Expression = _userServices.CreateExpression(x => new GetUserPaginationReponse(x));
            // filter
            var Filtet = _userServices.FilterUser(request.UserName, request.Email, request.City, request.Country, request.PostalCode, request.State, request.Street, request.OrederBy,request.userOredringEnum);

            var PaginationList = await Filtet.Select(Expression).ToPaginationListAsync(request.PageNumber, request.PageSize);

            PaginationList.Meta = new
            {
                Date = DateTime.Now.ToString(),
                Total = PaginationList.Data.Count()
            };

            return PaginationList;
        }

        public async Task<Response<GetUserByIdReponse>> Handle(GetUserByIdModel request, CancellationToken cancellationToken)
        {
            var User = await _userServices.GetUserById(request.Id);
            if (User == null) return NotFound<GetUserByIdReponse>($"Not Found User Wiht Id {request.Id}");
            var UserMapping = _mapper.Map<GetUserByIdReponse>(User);
            return Success(UserMapping);
        }

        public async Task<Response<List<GetSippingAddressUserResponse> >> Handle(GetSippingAddressUserModels request, CancellationToken cancellationToken)
        {
            if (request.userId.IsNullOrEmpty()) return BadRequest<List<GetSippingAddressUserResponse>>("Invaild Id");

            var shippingAddress = await _userServices.GetUserShippingAddress(request.userId);
            if(!shippingAddress.Any()) return NotFound<List<GetSippingAddressUserResponse>>("Not Found Address");

            var mappingAddress = shippingAddress.Select(x => new GetSippingAddressUserResponse {
                City = x.City,
                State = x.State,
                Country = x.Country,
                HouseNumber = x.HouseNumber,
                lat = x.lat,
                lon = x.lon,
                PostalCode = x.PostalCode,
                Street = x.Street,
                Suburb = x.Suburb,
                Id = x.AddressID
            }).ToList();


            return Success(mappingAddress);
 
        }
        #endregion


    }
}
