using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.AdministrativeCategories;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Configurations.AdministrativeCategories;

internal class MiningSourceConfiguration : IEntityTypeConfiguration<MiningSource>
{
    public void Configure(EntityTypeBuilder<MiningSource> builder)
    {
        builder.ToTable("MiningSources");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
    }
}
