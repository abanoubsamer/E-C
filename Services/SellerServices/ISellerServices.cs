using Domain.Dtos.Seller.Quereis;
using Domain.Enums.Oredring;
using Domain.Models;
using SchoolWep.Data.Enums.Oredring;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.SellerServices
{
    public interface ISellerServices
    {
        public IQueryable<ApplicationUser> FilterSeller(
        string? UserName,
        string? Email,
        string? City,
        string? Country,
        string? PostalCode,
        string? State,
        string? Street,
        OrederBy? orederBy,
        SellerOredringEnum? userOredringEnum
        );

        public Expression<Func<ApplicationUser, TResponse>> CreateExpression<TResponse>(Func<ApplicationUser, TResponse> func);

        public IQueryable<GetSelleProductsResponseDto> FilterSellerProduct(string sellerId, string? searchTerm);

        public Task<Seller> GetSellerById(string sellerId);

        public Task<bool> EmailIsExist(string Email);

        public Task<ProductListing> GetSellerProductById(string ProductID);
    }
}