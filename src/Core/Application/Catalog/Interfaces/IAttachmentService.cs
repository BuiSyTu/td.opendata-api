using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IAttachmentService : ITransientService
{
    Task<Result<List<AttachmentDto>>> CreateAsync(CreateAttachmentRequest request);
    Task<Result<Guid>> DeleteAsync(Guid id);
}