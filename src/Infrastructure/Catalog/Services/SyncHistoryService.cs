using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Common.Specifications;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog.SyncHistory;
using Microsoft.Extensions.Localization;
using Mapster;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using System.Linq.Expressions;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class SyncHistoryService : ISyncHistoryService
{
    private readonly IStringLocalizer<DataTypeService> _localizer;
    private readonly IRepositoryAsync _repository;

    public SyncHistoryService(IRepositoryAsync repository, IStringLocalizer<DataTypeService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<Result<SyncHistory>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<SyncHistory>(id);
        return await Result<SyncHistory>.SuccessAsync(item);
    }

    public async Task<PaginatedResult<SyncHistoryDto>> SearchAsync(SyncHistoryListFilter filter)
    {
        var filters = new Filters<SyncHistory>();
        filters.Add(filter.DatasetId.HasValue, x => x.DatasetId.Equals(filter.DatasetId!.Value));
        filters.Add(!string.IsNullOrEmpty(filter.DataTypeCode), x => x.Dataset.DataType.Code == filter.DataTypeCode);
        filters.Add(!string.IsNullOrEmpty(filter.OfficeCode), x => x.Dataset.OfficeCode == filter.OfficeCode);

        var specification = new PaginationSpecification<SyncHistory>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Filters = filters,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.SyncTime),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize,
            Includes = new Expression<Func<SyncHistory, object>>[] { x => x.Dataset }
        };

        return await _repository.GetListAsync<SyncHistory, SyncHistoryDto>(specification);
    }

    public async Task<int> CountAsync(SyncHistoryListFilter filter)
    {
        var filters = new Filters<SyncHistory>();
        filters.Add(filter.DatasetId.HasValue, x => x.DatasetId.Equals(filter.DatasetId!.Value));
        filters.Add(!string.IsNullOrEmpty(filter.DataTypeCode), x => x.Dataset.DataType.Code == filter.DataTypeCode);
        filters.Add(!string.IsNullOrEmpty(filter.OfficeCode), x => x.Dataset.OfficeCode == filter.OfficeCode);

        return await _repository.GetCountAsync(filters);
    }
}