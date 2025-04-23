using Domain.Enums.Oredring;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using SchoolWep.Data.Enums.Oredring;
using Services.Result;
using System.Linq.Expressions;

namespace Services.UserServices
{
    public interface IUserServices
    {
        public IQueryable<ApplicationUser> FilterUser(
         string? UserName,
         string? Email,
         string? City,
         string? Country,
         string? PostalCode,
         string? State,
         string? Street,
         OrederBy? orederBy,
         UserOredringEnum? userOredringEnum
         );
        public Expression<Func<ApplicationUser, TResponse>> CreateExpression<TResponse>(Func<ApplicationUser, TResponse> func);

        public Task<ApplicationUser> GetUserById(string Id);

        public Task<ResultServices> AddUserShippingAddress(ShippingAddress model);
        public Task<ResultServices> AddUserPhones(UserPhoneNumber model);
        public Task<ResultServices> UpdateUser(ApplicationUser user, IFormFile image);
        public Task<List<ShippingAddress>> GetUserShippingAddress(string Id);
        public Task<List<UserPhoneNumber>> GetUserPhones(string Id);
        public Task<bool> EmailIsExist(string Email);



    }
}
