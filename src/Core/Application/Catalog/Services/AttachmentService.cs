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

public class AttachmentService : IAttachmentService
{
    private readonly IStringLocalizer<AttachmentService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IJobService _jobService;

    public AttachmentService(IRepositoryAsync repository, IStringLocalizer<AttachmentService> localizer, IJobService jobService)
    {
        _repository = repository;
        _localizer = localizer;
        _jobService = jobService;
    }

    public async Task<Result<List<AttachmentDto>>> CreateAsync(CreateAttachmentRequest request)
    {
        var files = request.Files;


        return await Result<List<AttachmentDto>>.SuccessAsync(null);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<Tag>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }
}