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

public class DataTypeService : IDataTypeService
{
    private readonly IStringLocalizer<DataTypeService> _localizer;
    private readonly IRepositoryAsync _repository;

    public DataTypeService(IRepositoryAsync repository, IStringLocalizer<DataTypeService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> CreateAsync(CreateDataTypeRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<DataType>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["DataType.alreadyexists"], request.Name));
        var item = request.Adapt<DataType>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<DataType>(id);

        var codeCannotBeDelete = new List<string>()
        {
            "webapi",
            "excel"
        };

        if (codeCannotBeDelete.Contains(itemToDelete.Code.ToLower()))
        {
            throw new Exception("Item cannot be deleted because it is system item");
        }

        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<DataTypeDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<DataType, DataTypeDetailsDto>(id);
        return await Result<DataTypeDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<DataTypeDto>> SearchAsync(DataTypeListFilter filter)
    {
        var specification = new PaginationSpecification<DataType>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<DataType, DataTypeDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateDataTypeRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<DataType>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["DataType.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description, request.Code, request.IsActive);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

}