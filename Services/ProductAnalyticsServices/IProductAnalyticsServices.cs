using Domain.Models;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductAnalyticsServices
{
    public interface IProductAnalyticsServices
    {


        public Task<ResultServices> AddProductView(ProductView entity);
        public Task<ResultServices> AddProductInteraction(ProductInteraction entity);
        public  Task<int> GetProductViews(string productId);
        public  Task<int> GetSellerViews(string SellerId);
        public  Task<int> GetSellerPosts(string SellerId);
        public  Task<int> GetSellerComments(string SellerId);
        public Task<int> GetProductInteractions(string productId);
        public Task<int> GetSellerInteractions(string SellerId);
    }
}
