using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IOrganizationService : ITransientService
{
    Task<Result<OrganizationDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<OrganizationDto>> SearchAsync(OrganizationListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateOrganizationRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateOrganizationRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}