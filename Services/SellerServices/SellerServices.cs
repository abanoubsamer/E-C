using Domain.Dtos;
using Domain.Dtos.Seller.Quereis;
using Domain.Enums.Oredring;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolWep.Data.Enums.Oredring;
using System.Linq.Expressions;


namespace Services.SellerServices
{
    public class SellerServices : ISellerServices
    {

        #region Fialds 
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constractor
        public SellerServices(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        #endregion
        public Expression<Func<ApplicationUser, TResponse>> CreateExpression<TResponse>(Func<ApplicationUser, TResponse> func)
        {
            return x => func(x);
        }


        public async Task<bool> EmailIsExist(string Email)
        {
            return await _unitOfWork.Repository<ApplicationUser>().IsExistAsync(x => x.Email == Email && x.Seller != null);
        }

        public IQueryable<ApplicationUser> FilterSeller(
            string? UserName,
            string? Email,
            string? City,
            string? Country,
            string? PostalCode,
            string? State,
            string? Street,
            OrederBy? orderBy,
            SellerOredringEnum? userOrderingEnum)
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


        public IQueryable<GetSelleProductsResponseDto> FilterSellerProduct(string sellerId, string? searchTerm)
        {
            if (string.IsNullOrEmpty(sellerId))
                return Enumerable.Empty<GetSelleProductsResponseDto>().AsQueryable(); 

            var query = GetQueryableSellerProducts(sellerId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.id.Contains(searchTerm) ||
                    x.name.Contains(searchTerm) ||
                    x.descreption.Contains(searchTerm) ||
                    x.category.Name.Contains(searchTerm) ||
                    x.price.ToString().Contains(searchTerm) ||
                    x.stock.ToString().Contains(searchTerm)
                );
            }

            return query;
        }


        public IQueryable<ApplicationUser> Oreder(IQueryable<ApplicationUser> Queres, OrederBy? orderBy,
            SellerOredringEnum? userOrderingEnum)
        {
            userOrderingEnum = userOrderingEnum == null ? 0 : userOrderingEnum;
            bool Asending = orderBy == null || orderBy == 0;


            switch (userOrderingEnum)
            {
                case SellerOredringEnum.Name:
                    Queres = Asending ? Queres.OrderBy(x => x.Name) : Queres.OrderByDescending(x => x.Name);
                    break;
                case SellerOredringEnum.Email:
                    Queres = Asending ? Queres.OrderBy(x => x.Email) : Queres.OrderByDescending(x => x.Email);
                    break;
            }

            return Queres;

        }

        private IQueryable<GetSelleProductsResponseDto> GetQueryableSellerProducts(string sellerid)
        {
            var reviewsQuery = _unitOfWork.Repository<Domain.Models.Review>()
                .GetQueryable()
                .AsNoTracking()
                .GroupBy(r => r.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    AverageRating = g.Average(r => r.Rating)
                });

            return _unitOfWork.Repository<Domain.Models.Product>()
                .GetQueryable()
                .AsNoTracking()
                .Where(x => x.SellerID == sellerid)
                .Select(product => new GetSelleProductsResponseDto
                {
                    id = product.ProductID,
                    name = product.Name,
                    descreption = product.Description,
 
                    price = product.Price,
                    stock = product.StockQuantity,

                    avaragarate = reviewsQuery
                        .Where(r => r.ProductID == product.ProductID)
                        .Select(r => (double?)r.AverageRating)
                        .FirstOrDefault() ?? 0, 

                    ProductImagesDto = product.Images.Select(x => new ProductImagesDto
                    {
                        id = x.ImageID,
                        Image = x.ImageUrl,
                    }).ToList(),

                    category = new CategoryDto
                    {
                        Id = product.Category.CategoryID,
                        Name = product.Category.Name
                    },
                });
        }



        private IQueryable<ApplicationUser> GetQueryable()
        {
            return _unitOfWork.Repository<ApplicationUser>()
                .GetQueryable()
                .Where(x => x.Seller != null)
                .Include(x => x.ShippingAddresses);
        }

        public async Task<Seller> GetSellerById(string sellerId)
        {
            return await _unitOfWork.Repository<Seller>().FindOneWithNoTrackingAsync(x => x.SellerID == sellerId);
        }
    }
}
