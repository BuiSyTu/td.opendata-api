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
using TD.OpenData.WebApi.Application.FileStorage;
using TD.OpenData.WebApi.Domain.Common;
using System.Linq.Expressions;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class CategoryService : ICategoryService
{
    private readonly IStringLocalizer<CategoryService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IJobService _jobService;
    private readonly IFileStorageService _file;

    public CategoryService(IRepositoryAsync repository, IStringLocalizer<CategoryService> localizer, IJobService jobService, IFileStorageService file)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
        _file = file;
    }

    public async Task<Result<Guid>> CreateAsync(CreateCategoryRequest request)
    {
        if (request.ParentId != default)
        {
            bool brandExists = await _repository.ExistsAsync<Brand>(a => a.Id == request.ParentId);
            if (!brandExists) throw new EntityNotFoundException(string.Format(_localizer["category.notfound"], request.ParentId));
        }

        bool itemExists = await _repository.ExistsAsync<Category>(a => a.Name == request.Name);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["category.alreadyexists"], request.Name));
        string itemImagePath = await _file.UploadAsync<Category>(request.Image, FileType.Image);
        var item = request.Adapt<Category>();
        item.ImageUrl = itemImagePath;

        item.DomainEvents.Add(new StatsChangedEvent());
        var itemId = await _repository.CreateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(itemId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<Category>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<CategoryDetailsDto>> GetDetailsAsync(Guid id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var includes = new Expression<Func<Category, object>>[] { x => x.Parent };
#pragma warning restore CS8603 // Possible null reference return.
        var item = await _repository.GetByIdAsync<Category, CategoryDetailsDto>(id, includes);
        return await Result<CategoryDetailsDto>.SuccessAsync(item);
    }

    public Task<PaginatedResult<CategoryDto>> SearchAsync(CategoryListFilter filter)
    {
        var specification = new PaginationSpecification<Category>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<Category, CategoryDto>(specification);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateCategoryRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<Category>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["category.notfound"], id));
        string? itemImagePath = null;
        if (request.Image != null) itemImagePath = await _file.UploadAsync<Category>(request.Image, FileType.Image);

        if (request.ParentId != default)
        {
            var parentItem = await _repository.GetByIdAsync<Category>((Guid)request.ParentId, null);
            if (parentItem == null) throw new EntityNotFoundException(string.Format(_localizer["category.notfound"], id));
        }

        var updatedItem = item.Update(request.ParentId, request.Name, request.Code, request.Description, itemImagePath, request.Icon, request.Order, request.IsActive);
        updatedItem.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedItem);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

}