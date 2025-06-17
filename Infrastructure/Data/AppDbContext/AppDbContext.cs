using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos.Product.Queries;

namespace Infrastructure.Data.AppDbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Seller> Sellers { get; set; }
        public DbSet<UserFCMToken> UserFCMTokens { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<CardItem> CardItem { get; set; }
        public DbSet<ProductImages> productImages { get; set; }
        public DbSet<ProductListing> Products { get; set; }
        public DbSet<ProductMaster> ProductsMaster { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserPhoneNumber> UserPhoneNumbers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<ModelCompatibility> ModelCompatibilitys { get; set; }
        public DbSet<Models> Models { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}