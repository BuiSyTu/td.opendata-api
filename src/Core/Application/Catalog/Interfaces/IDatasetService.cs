using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IDatasetService : ITransientService
{
    Task<Result<MetadataDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<MetadataDto>> SearchAsync(DatasetListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateDatasetRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateCategoryRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}