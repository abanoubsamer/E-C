using Domain.Models;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.MetaData.Routing;

namespace Services.ProductAnalyticsServices
{
    public class ProductAnalyticsServices : IProductAnalyticsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductAnalyticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultServices> AddProductInteraction(ProductInteraction entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Enity" };
            try
            {
                await _unitOfWork.Repository<ProductInteraction>().AddAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<ResultServices> AddProductView(ProductView entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Enity" };
            try
            {
                await _unitOfWork.Repository<ProductView>().AddAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }

        public async Task<int> GetProductViews(string productId)
        {
            return _unitOfWork.Repository<ProductView>().GetQueryable().AsNoTracking().Count(v => v.ProductId == productId);
        }

        public async Task<int> GetProductInteractions(string productId)
        {
            return _unitOfWork.Repository<ProductInteraction>().GetQueryable().AsNoTracking().Count(v => v.ProductId == productId);
        }

        public async Task<int> GetSellerViews(string SellerId)
        {
            return _unitOfWork.Repository<ProductView>().GetQueryable().AsNoTracking().Count(x => x.Product.SellerID == SellerId);
        }

        public async Task<int> GetSellerInteractions(string SellerId)
        {
            return _unitOfWork.Repository<ProductInteraction>().GetQueryable().AsNoTracking().Count(x => x.Product.SellerID == SellerId);
        }

        public async Task<int> GetSellerPosts(string SellerId)
        {
            return _unitOfWork.Repository<Domain.Models.ProductListing>().GetQueryable().AsNoTracking().Count(x => x.SellerID == SellerId);
        }

        public async Task<int> GetSellerComments(string SellerId)
        {
            return _unitOfWork.Repository<Domain.Models.Review>().GetQueryable().AsNoTracking().Count(x => x.Product.SellerID == SellerId);
        }
    }
}