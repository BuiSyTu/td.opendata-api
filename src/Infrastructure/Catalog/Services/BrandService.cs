using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Common.Specifications;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.Extensions.Localization;
using Hangfire;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class BrandService : IBrandService
{
    private readonly IStringLocalizer<BrandService> _localizer;
    private readonly IRepositoryAsync _repository;

    public BrandService(IRepositoryAsync repository, IStringLocalizer<BrandService> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> CreateBrandAsync(CreateBrandRequest request)
    {
        bool brandExists = await _repository.ExistsAsync<Brand>(a => a.Name == request.Name);
        if (brandExists) throw new EntityAlreadyExistsException(string.Format(_localizer["brand.alreadyexists"], request.Name));
        var brand = new Brand(request.Name, request.Description);
        brand.DomainEvents.Add(new StatsChangedEvent());
        var brandId = await _repository.CreateAsync(brand);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(brandId);
    }

    public async Task<Result<Guid>> DeleteBrandAsync(Guid id)
    {
        bool isBrandUsed = await _repository.ExistsAsync<Product>(a => a.BrandId == id);
        if (isBrandUsed) throw new EntityCannotBeDeleted(_localizer["brand.cannotbedeleted"]);
        var brandToDelete = await _repository.RemoveByIdAsync<Brand>(id);
        brandToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public Task<PaginatedResult<BrandDto>> SearchAsync(BrandListFilter filter)
    {
        var specification = new PaginationSpecification<Brand>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize
        };

        return _repository.GetListAsync<Brand, BrandDto>(specification);
    }

    public async Task<Result<Guid>> UpdateBrandAsync(UpdateBrandRequest request, Guid id)
    {
        var brand = await _repository.GetByIdAsync<Brand>(id);
        if (brand == null) throw new EntityNotFoundException(string.Format(_localizer["brand.notfound"], id));
        var updatedBrand = brand.Update(request.Name, request.Description);
        updatedBrand.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(updatedBrand);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public Task<Result<string>> GenerateRandomBrandAsync(GenerateRandomBrandRequest request)
    {
        string jobId = BackgroundJob.Enqueue<IBrandGeneratorJob>(x => x.GenerateAsync(request.NSeed));
        return Result<string>.SuccessAsync(jobId);
    }

    public Task<Result<string>> DeleteRandomBrandAsync()
    {
        string jobId = BackgroundJob.Schedule<IBrandGeneratorJob>(x => x.CleanAsync(), TimeSpan.FromSeconds(5));
        return Result<string>.SuccessAsync(jobId);
    }
}