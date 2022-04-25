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
using TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DocumentType;

namespace TD.OpenData.WebApi.Infrastructure.AdministrativeCategories.Services;

public class DocumentTypeService : IDocumentTypeService
{
    private readonly IStringLocalizer<DocumentTypeService> _localizer;
    private readonly IRepositoryAsync _repository;

    public DocumentTypeService(IRepositoryAsync repository, IStringLocalizer<DocumentTypeService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> CreateAsync(CreateDocumentTypeRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<DocumentType>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["DataType.alreadyexists"], request.Name));
        var item = request.Adapt<DocumentType>();

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<DocumentType>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<DocumentTypeDetailsDto>> GetDetailsAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<DocumentType, DocumentTypeDetailsDto>(id);
        return await Result<DocumentTypeDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<DocumentTypeDto>> SearchAsync(DocumentTypeListFilter filter)
    {
        var specification = new PaginationSpecification<DocumentType>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<DocumentType, DocumentTypeDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateDocumentTypeRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<DocumentType>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["DataType.notfound"], id));
        var updatedItem = item.Update(request.Name, request.Description);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }
}
