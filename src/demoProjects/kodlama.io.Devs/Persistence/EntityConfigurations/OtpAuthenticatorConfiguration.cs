using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class OtpAuthenticatorConfiguration : IEntityTypeConfiguration<OtpAuthenticator>
    {
        public void Configure(EntityTypeBuilder<OtpAuthenticator> builder)
        {
            builder.ToTable("OtpAuthenticators").HasKey(x => x.Id);
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.SecretKey).HasColumnName("SecretKey");
            builder.Property(x => x.IsVerified).HasColumnName("IsVerified");
            builder.HasOne(x => x.User);

        }
    }
}
