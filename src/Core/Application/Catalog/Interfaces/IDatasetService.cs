using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IDatasetService : ITransientService
{
    Task<object> GetDataAsync(Guid id, string orderBy, int skip, int top);

    Task<object> GetRawAsync(Guid id);

    Task<Result<DatasetDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<DatasetDto>> SearchAsync(DatasetListFilter filter);

    Task<int> GetCountAsync(DatasetListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateDatasetRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateDatasetRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);

    Task<Result<Guid>> ApprovedAsync(Guid id);

    Task<Result<Guid>> RejectedAsync(Guid id);

    Task SyncDataAsync(Guid id);

    object GroupByCategory();

    object GroupByOrganization();
}