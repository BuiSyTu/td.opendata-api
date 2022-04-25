using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Configurations.Catalog;

internal class DatasetAPIConfigConfiguration : IEntityTypeConfiguration<DatasetAPIConfig>
{
    public void Configure(EntityTypeBuilder<DatasetAPIConfig> builder)
    {
        builder.ToTable("DatasetAPIConfigs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
    }
}
