using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<ProductListing>
    {
        public void Configure(EntityTypeBuilder<ProductListing> builder)
        {
            builder.Property(o => o.ProductID)
                .HasDefaultValueSql("NEWID()");
        }
    }
}