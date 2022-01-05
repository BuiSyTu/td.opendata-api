using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Configurations;

internal class ProviderTypeConfiguration : IEntityTypeConfiguration<ProviderType>
{
    public void Configure(EntityTypeBuilder<ProviderType> builder)
    {
        builder.ToTable("ProviderTypes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Code).HasMaxLength(250);
    }
}
