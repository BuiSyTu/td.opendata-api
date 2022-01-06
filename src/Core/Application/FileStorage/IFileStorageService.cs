using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.FileStorage;

namespace TD.OpenData.WebApi.Application.FileStorage;

public interface IFileStorageService : ITransientService
{
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType)
    where T : class;

    public Task<List<AttachmentDto>> UploadFilesAsync<T>(CreateAttachmentRequest? request)
    where T : class;

    public void Remove(string? path);
}