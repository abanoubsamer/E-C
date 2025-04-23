using Domain.Enums.Oredring;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolWep.Data.Enums.Oredring;
using Services.ExtinsionServies;
using Services.FileSystemServices;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.UserServices
{
    public class UserServices : IUserServices
    {
        #region Filads

        private readonly UserManager<ApplicationUser> _identityUser;
        private readonly IFileServices fileServices;
        private readonly IUnitOfWork _unitOfWork;

        #endregion Filads

        #region Constractor

        public UserServices(UserManager<ApplicationUser> identityUser, IFileServices fileServices, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _identityUser = identityUser;
            this.fileServices = fileServices;
        }

        #endregion Constractor

        #region Implemntation

        public async Task<bool> EmailIsExist(string Email)
        {
            return await _unitOfWork.Repository<ApplicationUser>().IsExistAsync(x => x.Email == Email);
        }

        public async Task<ResultServices> AddUserShippingAddress(ShippingAddress model)
        {
            if (model == null) return new ResultServices { Msg = "Invalid Userid Or ShippingAddress" };

            try
            {
                await _unitOfWork.Repository<ShippingAddress>().AddAsync(model);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.InnerException.Message };
            }
        }

        public Expression<Func<ApplicationUser, TResponse>> CreateExpression<TResponse>(Func<ApplicationUser, TResponse> func)
        {
            return x => func(x);
        }

        public IQueryable<ApplicationUser> FilterUser(
            string? UserName,
            string? Email,
            string? City,
            string? Country,
            string? PostalCode,
            string? State,
            string? Street,
            OrederBy? orderBy,
            UserOredringEnum? userOrderingEnum)
        {
            var query = GetQueryable(); // Assuming GetQueryable() returns IQueryable<ApplicationUser>

            // Applying filters based on input parameters
            if (!string.IsNullOrEmpty(UserName))
            {
                query = query.Where(x => x.UserName.Contains(UserName));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                query = query.Where(x => x.Email.Contains(Email));
            }

            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => x.ShippingAddresses.Any(a => a.City.Contains(City)));
            }

            if (!string.IsNullOrEmpty(Country))
            {
                query = query.Where(x => x.ShippingAddresses.Any(a => a.Country.Contains(Country)));
            }

            if (!string.IsNullOrEmpty(PostalCode))
            {
                query = query.Where(x => x.ShippingAddresses.Any(a => a.PostalCode.Contains(PostalCode)));
            }

            if (!string.IsNullOrEmpty(State))
            {
                query = query.Where(x => x.ShippingAddresses.Any(a => a.State.Contains(State)));
            }

            if (!string.IsNullOrEmpty(Street))
            {
                query = query.Where(x => x.ShippingAddresses.Any(a => a.Street.Contains(Street)));
            }

            return Oreder(query, orderBy, userOrderingEnum);
        }

        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return await _unitOfWork.Repository<ApplicationUser>().FindOneWithNoTrackingAsync(x => x.Id == Id);
        }

        public IQueryable<ApplicationUser> Oreder(IQueryable<ApplicationUser> Queres, OrederBy? orderBy,
            UserOredringEnum? userOrderingEnum)
        {
            userOrderingEnum = userOrderingEnum == null ? 0 : userOrderingEnum;
            bool Asending = orderBy == null || orderBy == 0;

            switch (userOrderingEnum)
            {
                case UserOredringEnum.Name:
                    Queres = Asending ? Queres.OrderBy(x => x.Name) : Queres.OrderByDescending(x => x.Name);
                    break;

                case UserOredringEnum.Email:
                    Queres = Asending ? Queres.OrderBy(x => x.Email) : Queres.OrderByDescending(x => x.Email);
                    break;
            }

            return Queres;
        }

        private IQueryable<ApplicationUser> GetQueryable()
        {
            return _unitOfWork.Repository<ApplicationUser>()
                .GetQueryable()
                .Where(x => x.Seller == null)
                .Include(x => x.ShippingAddresses);
        }

        public async Task<List<ShippingAddress>> GetUserShippingAddress(string Id)
        {
            return await _unitOfWork.Repository<ShippingAddress>().FindMoreAsNoTrackingAsync(x => x.ApplicationUserId == Id);
        }

        public async Task<ResultServices> AddUserPhones(UserPhoneNumber model)
        {
            if (model == null) return new ResultServices { Msg = "Invalid Userid Or ShippingAddress" };

            try
            {
                var isexist = await _unitOfWork.Repository<UserPhoneNumber>().IsExistAsync(x => x.PhoneNumber == model.PhoneNumber);
                if (isexist) return new ResultServices { Msg = "Phone Aready Exist" };
                await _unitOfWork.Repository<UserPhoneNumber>().AddAsync(model);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.InnerException.Message };
            }
        }

        public async Task<List<UserPhoneNumber>> GetUserPhones(string Id)
        {
            return await _unitOfWork.Repository<UserPhoneNumber>().FindMoreAsNoTrackingAsync(x => x.UserId == Id);
        }

        public async Task<ResultServices> UpdateUser(ApplicationUser user, IFormFile image)
        {
            if (user == null) return new ResultServices { Msg = "Invald User" };
            var ImageResult = await this.fileServices.AddImageAsync("wwwroot/images/User", image);
            if (!ImageResult.Succesd)
            {
                return new ResultServices { Msg = ImageResult.Msg };
            }
            if (user.Picture != null)
            {
                var deleteResult = await this.fileServices.DeleteImageAsync(user.Picture, "User");
                if (!deleteResult.Succesd) return new ResultServices { Msg = deleteResult.Msg };
            }

            try
            {
                user.Picture = ImageResult.Msg;
                var result = await _identityUser.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description} :";
                    }
                    return new ResultServices { Msg = errors };
                }

                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                await this.fileServices.DeleteImageAsync($"User/{ImageResult.Msg}");
                return new ResultServices { Msg = ex.Message };
            }
        }

        #endregion Implemntation
    }
}