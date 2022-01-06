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

namespace TD.OpenData.WebApi.Application.Catalog.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IStringLocalizer<AttachmentService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IJobService _jobService;
    private readonly IFileStorageService _file;


    public AttachmentService(IRepositoryAsync repository, IStringLocalizer<AttachmentService> localizer, IJobService jobService, IFileStorageService file)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
        _file = file;
    }

    public async Task<Result<List<AttachmentDto>>> CreateAsync(CreateAttachmentRequest request)
    {
        var listFile = await _file.UploadFilesAsync<Attachment>(request);

        return await Result<List<AttachmentDto>>.SuccessAsync(listFile);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<Tag>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }
}