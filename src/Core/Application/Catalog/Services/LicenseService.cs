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

public class LicenseService : ILicenseService
{
    private readonly IStringLocalizer<LicenseService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IJobService _jobService;

    public LicenseService(IRepositoryAsync repository, IStringLocalizer<LicenseService> localizer, IJobService jobService)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
    }

    public async Task<Result<Guid>> CreateAsync(CreateLicenseRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<License>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["License.alreadyexists"], request.Name));
        var item = request.Adapt<License>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<License>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<LicenseDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<License, LicenseDetailsDto>(id);
        return await Result<LicenseDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<LicenseDto>> SearchAsync(LicenseListFilter filter)
    {
        var specification = new PaginationSpecification<License>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<License, LicenseDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateLicenseRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<License>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["License.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description, request.Code, request.IsActive);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

}