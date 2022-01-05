using TD.OpenData.WebApi.Domain.Multitenancy;
using Microsoft.EntityFrameworkCore;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

public class TenantManagementDbContext : DbContext
{
    public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
    : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>()
            .Property(t => t.Issuer)
            .HasMaxLength(256);
    }
}