using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Configurations;

internal class DatasetOfficeConfiguration : IEntityTypeConfiguration<DatasetOffice>
{
    public void Configure(EntityTypeBuilder<DatasetOffice> builder)
    {
        builder.ToTable("DatasetOffices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
    }
}
