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

public class OrganizationService : IOrganizationService
{
    private readonly IStringLocalizer<OrganizationService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IJobService _jobService;

    public OrganizationService(IRepositoryAsync repository, IStringLocalizer<OrganizationService> localizer, IJobService jobService)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
    }

    public async Task<Result<Guid>> CreateAsync(CreateOrganizationRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<Organization>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["Organization.alreadyexists"], request.Name));
        var item = request.Adapt<Organization>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<Organization>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }



    public async Task<Result<OrganizationDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<Organization, OrganizationDetailsDto>(id);
        return await Result<OrganizationDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<OrganizationDto>> SearchAsync(OrganizationListFilter filter)
    {
        var specification = new PaginationSpecification<Organization>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<Organization, OrganizationDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateOrganizationRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<Organization>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["Organization.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description, request.Code, request.IsActive);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

}