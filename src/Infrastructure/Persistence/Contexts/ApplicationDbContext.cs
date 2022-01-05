using System.Data;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Application.Multitenancy;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using TD.OpenData.WebApi.Infrastructure.Persistence.Configurations;

namespace TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : BaseDbContext
{
    private readonly IEventService _eventService;
    public IDbConnection Connection => Database.GetDbConnection();
    private readonly ICurrentUser _currentUserService;

    public ApplicationDbContext(DbContextOptions options, ITenantService tenantService, ICurrentUser currentUserService, ISerializerService serializer, IEventService eventService)
    : base(options, tenantService, currentUserService, serializer)
    {
        _currentUserService = currentUserService;
        _eventService = eventService;
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CustomField> CustomFields => Set<CustomField>();
    public DbSet<Dataset> Datasets => Set<Dataset>();
    public DbSet<DatasetAPIConfig> DatasetAPIConfigs => Set<DatasetAPIConfig>();
    public DbSet<DatasetDBConfig> DatasetDBConfigs => Set<DatasetDBConfig>();
    public DbSet<DatasetFileConfig> DatasetFileConfigs => Set<DatasetFileConfig>();
    public DbSet<DataType> DataTypes => Set<DataType>();
    public DbSet<License> Licenses => Set<License>();
    public DbSet<Metadata> Metadatas => Set<Metadata>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<ProviderType> ProviderTypes => Set<ProviderType>();
    public DbSet<Tag> Tags => Set<Tag>();


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        string? currentUserId = _currentUserService.GetUserName();
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.LastModifiedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = currentUserId;
                        softDelete.DeletedOn = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
        }

        int results = await base.SaveChangesAsync(cancellationToken);
        if (_eventService == null) return results;
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                                                .Select(e => e.Entity)
                                                .Where(e => e.DomainEvents.Count > 0)
                                                .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var @event in events)
            {
                await _eventService.PublishAsync(@event);
            }
        }

        return results;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new CustomFieldConfiguration());

        builder.ApplyConfiguration(new DatasetAPIConfigConfiguration());

        builder.ApplyConfiguration(new DatasetConfiguration());

        builder.ApplyConfiguration(new DatasetDBConfigConfiguration());

        builder.ApplyConfiguration(new DatasetFileConfigConfiguration());

        builder.ApplyConfiguration(new DataTypeConfiguration());

        builder.ApplyConfiguration(new LicenseConfiguration());

        builder.ApplyConfiguration(new MetadataConfiguration());

        builder.ApplyConfiguration(new OrganizationConfiguration());

        builder.ApplyConfiguration(new ProviderTypeConfiguration());

        builder.ApplyConfiguration(new TagConfiguration());

        base.OnModelCreating(builder);
    }
}