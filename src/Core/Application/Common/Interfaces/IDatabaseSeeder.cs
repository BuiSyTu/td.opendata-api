using TD.OpenData.WebApi.Domain.Multitenancy;

namespace TD.OpenData.WebApi.Application.Common.Interfaces;

public interface IDatabaseSeeder
{
    void Initialize(Tenant tenant);
}