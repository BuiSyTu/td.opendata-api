using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Common.Specifications;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.Extensions.Localization;
using Mapster;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class ProviderTypeService : IProviderTypeService
{
    private readonly IStringLocalizer<ProviderTypeService> _localizer;
    private readonly IRepositoryAsync _repository;

    public ProviderTypeService(IRepositoryAsync repository, IStringLocalizer<ProviderTypeService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
    }

    public async Task<Result<Guid>> CreateAsync(CreateProviderTypeRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<ProviderType>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["ProviderType.alreadyexists"], request.Name));
        var item = request.Adapt<ProviderType>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<ProviderType>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<ProviderTypeDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<ProviderType, ProviderTypeDetailsDto>(id);
        return await Result<ProviderTypeDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<ProviderTypeDto>> SearchAsync(ProviderTypeListFilter filter)
    {
        var specification = new PaginationSpecification<ProviderType>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<ProviderType, ProviderTypeDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateProviderTypeRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<ProviderType>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["ProviderType.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description, request.Code, request.IsActive);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

}