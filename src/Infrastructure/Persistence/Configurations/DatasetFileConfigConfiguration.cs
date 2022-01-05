using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Configurations;

internal class DatasetFileConfigConfiguration : IEntityTypeConfiguration<DatasetFileConfig>
{
    public void Configure(EntityTypeBuilder<DatasetFileConfig> builder)
    {
        builder.ToTable("DatasetFileConfigs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");

    }
}
