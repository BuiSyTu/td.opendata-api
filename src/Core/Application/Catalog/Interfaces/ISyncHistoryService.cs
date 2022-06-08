using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog.SyncHistory;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface ISyncHistoryService : ITransientService
{
    Task<Result<SyncHistory>> GetDetailsAsync(Guid id);
    Task<PaginatedResult<SyncHistoryDto>> SearchAsync(SyncHistoryListFilter filter);
    Task<int> CountAsync(SyncHistoryListFilter filter);
}