using AutoMapper;
using Core.Basic;
using Core.Meditor.User.Commend.Models;
using Domain.Models;
using MediatR;
using Services.ExtinsionServies;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.User.Commend.handler
{
    public class UserHanlderCommend:ResponseHandler,
        IRequestHandler<AddUserShippingAddressesModel,Response<string>>,
        IRequestHandler<UpdateUserModel, Response<string>>,
        IRequestHandler<AddUserPhonesModel, Response<string>>
    {


        #region Fialds
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor
        public UserHanlderCommend(IUserServices userServices, IMapper mapper)
        {
            _mapper = mapper;
            _userServices = userServices;
        }


        #endregion

        #region Handler
        public async Task<Response<string>> Handle(AddUserShippingAddressesModel request, CancellationToken cancellationToken)
        {

            var AddresMapper = _mapper.Map<ShippingAddress>(request);


            var result = await _userServices.AddUserShippingAddress(AddresMapper);
            
            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Created<string>(AddresMapper.AddressID);
            
        }

        public async Task<Response<string>> Handle(AddUserPhonesModel request, CancellationToken cancellationToken)
        {
            if (request.UserId.IsNullOrEmpty() || request.Phone.IsNullOrEmpty()) return BadRequest<string>("Invalid Request");

            var UserPhone = new UserPhoneNumber
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = request.Phone,
                UserId = request.UserId
            };
            var result = await _userServices.AddUserPhones(UserPhone);
            if (!result.Succesd) return BadRequest<string>(result.Msg);
            return Success(UserPhone.Id);
        }

        public async Task<Response<string>> Handle(UpdateUserModel request, CancellationToken cancellationToken)
        {
           var user = await _userServices.GetUserById(request.UserId);
            if (user == null) return NotFound<string>("Not Found User");
        
            var result = await _userServices.UpdateUser(user , request.FormImages);
            if (!result.Succesd) return BadRequest<string>(result.Msg);

            return Updated<string>(user.Picture);
        }
        #endregion

    }
}
