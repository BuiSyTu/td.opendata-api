using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface ICategoryService : ITransientService
{
    Task<Result<CategoryDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<CategoryDto>> SearchAsync(CategoryListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateCategoryRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateCategoryRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}