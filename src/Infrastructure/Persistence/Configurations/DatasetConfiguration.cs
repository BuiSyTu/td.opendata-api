using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Configurations;

internal class DatasetConfiguration : IEntityTypeConfiguration<Dataset>
{
    public void Configure(EntityTypeBuilder<Dataset> builder)
    {
        builder.ToTable("Datasets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Code).HasMaxLength(250);
        builder.HasOne<License>(x => x.License).WithMany(s => s.Datasets).HasForeignKey(x => x.LicenseId);
        builder.HasOne<DataType>(x => x.DataType).WithMany(s => s.Datasets).HasForeignKey(x => x.DataTypeId);
        builder.HasOne<Organization>(x => x.Organization).WithMany(s => s.Datasets).HasForeignKey(x => x.OrganizationId);
        builder.HasOne<ProviderType>(x => x.ProviderType).WithMany(s => s.Datasets).HasForeignKey(x => x.ProviderTypeId);
        builder.HasOne<Category>(x => x.Category).WithMany(s => s.Datasets).HasForeignKey(x => x.CategoryId);

        builder.HasOne<DatasetAPIConfig>(s => s.DatasetAPIConfig).WithOne(ad => ad.Dataset).HasForeignKey<DatasetAPIConfig>(ad => ad.DatasetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<DatasetFileConfig>(s => s.DatasetFileConfig).WithOne(ad => ad.Dataset).HasForeignKey<DatasetFileConfig>(ad => ad.DatasetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<DatasetDBConfig>(s => s.DatasetDBConfig).WithOne(ad => ad.Dataset).HasForeignKey<DatasetDBConfig>(ad => ad.DatasetId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Metadata>(x => x.Metadatas).WithOne(s => s.Dataset).HasForeignKey(s => s.DatasetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany<DatasetOffice>(x => x.DatasetOffices).WithOne(s => s.Dataset).HasForeignKey(s => s.DatasetId).OnDelete(DeleteBehavior.Cascade);

    }

}
