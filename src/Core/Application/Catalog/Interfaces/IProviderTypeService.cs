using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IProviderTypeService : ITransientService
{
    Task<Result<ProviderTypeDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<ProviderTypeDto>> SearchAsync(ProviderTypeListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateProviderTypeRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateProviderTypeRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}