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
using TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.MiningSource;

namespace TD.OpenData.WebApi.Infrastructure.AdministrativeCategories.Services;

public class MiningSourceService : IMiningSourceService
{
    private readonly IStringLocalizer<MiningSourceService> _localizer;
    private readonly IRepositoryAsync _repository;

    public MiningSourceService(IRepositoryAsync repository, IStringLocalizer<MiningSourceService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> CreateAsync(CreateMiningSourceRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<MiningSource>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["DataType.alreadyexists"], request.Name));
        var item = request.Adapt<MiningSource>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<MiningSource>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<MiningSourceDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<MiningSource, MiningSourceDetailsDto>(id);
        return await Result<MiningSourceDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<MiningSourceDto>> SearchAsync(MiningSourceListFilter filter)
    {
        var specification = new PaginationSpecification<MiningSource>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<MiningSource, MiningSourceDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateMiningSourceRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<MiningSource>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["DataType.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }
}
