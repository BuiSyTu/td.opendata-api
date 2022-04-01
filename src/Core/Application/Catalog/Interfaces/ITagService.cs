using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface ITagService : ITransientService
{
    Task<Result<TagDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<TagDto>> SearchAsync(TagListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateTagRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateTagRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}