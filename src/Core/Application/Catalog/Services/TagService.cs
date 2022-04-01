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

namespace TD.OpenData.WebApi.Application.Catalog.Services;

public class TagService : ITagService
{
    private readonly IStringLocalizer<TagService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IJobService _jobService;

    public TagService(IRepositoryAsync repository, IStringLocalizer<TagService> localizer, IJobService jobService)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
    }

    public async Task<Result<Guid>> CreateAsync(CreateTagRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<Tag>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["Tag.alreadyexists"], request.Name));
        var item = request.Adapt<Tag>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<Tag>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<TagDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<Tag, TagDetailsDto>(id);
        return await Result<TagDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<TagDto>> SearchAsync(TagListFilter filter)
    {
        var specification = new PaginationSpecification<Tag>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<Tag, TagDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateTagRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<Tag>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["Tag.notfound"], id));
        var updatedItem = item.Update(request.Name);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

}