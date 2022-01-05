using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Multitenancy;

namespace TD.OpenData.WebApi.Application.Multitenancy;

public interface ITenantService : IScopedService
{
    public string? GetDatabaseProvider();

    public string? GetConnectionString();

    public TenantDto? GetCurrentTenant();

    public void SetCurrentTenant(string tenant);
}