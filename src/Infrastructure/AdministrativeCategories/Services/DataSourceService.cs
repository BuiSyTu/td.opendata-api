using Mapster;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.AdministrativeCategories.Interfaces;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Common.Specifications;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.AdministrativeCategories;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DataSource;

namespace TD.OpenData.WebApi.Infrastructure.AdministrativeCategories.Services;

public class DataSourceService : IDataSourceService
{
    private readonly IStringLocalizer<DataSourceService> _localizer;
    private readonly IRepositoryAsync _repository;

    public DataSourceService(IRepositoryAsync repository, IStringLocalizer<DataSourceService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> CreateAsync(CreateDataSourceRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<DataSource>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["DataSource.alreadyexists"], request.Name));
        var item = request.Adapt<DataSource>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<DataSource>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<DataSourceDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<DataSource, DataSourceDetailsDto>(id);
        return await Result<DataSourceDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<DataSourceDto>> SearchAsync(DataSourceListFilter filter)
    {
        var specification = new PaginationSpecification<DataSource>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<DataSource, DataSourceDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateDataSourceRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<DataSource>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["DataSource.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }
}
