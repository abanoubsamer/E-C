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
    public class UserPhoneNumberConfig : IEntityTypeConfiguration<UserPhoneNumber>
    {
        public void Configure(EntityTypeBuilder<UserPhoneNumber> builder)
        {
            builder.Property(x => x.PhoneNumber).HasDefaultValueSql("NEWID()");
        }
    }
}
