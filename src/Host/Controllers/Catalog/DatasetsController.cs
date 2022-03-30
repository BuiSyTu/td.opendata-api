using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TD.OpenData.WebApi.Application.Forward.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;
using TD.OpenData.WebApi.Application.Common.Interfaces;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class DatasetsController : BaseController
{
    private readonly IRepositoryAsync _repository;
    private readonly IDatasetService _service;
    private readonly IForwardService _forwardService;
    private readonly ISqlService _sqlService;

    public DatasetsController(
        IRepositoryAsync repository,
        IDatasetService service,
        IForwardService forwardService,
        ISqlService sqlService)
    {
        _repository = repository;
        _service = service;
        _forwardService = forwardService;
        _sqlService = sqlService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] DatasetListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var product = await _service.GetDetailsAsync(id);
        return Ok(product);
    }

    [HttpPost("search")]
    [OpenApiOperation("Danh sách dataset.", "")]
    public async Task<IActionResult> SearchAsync(DatasetListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateDatasetRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(UpdateDatasetRequest request, Guid id)
    {
        return Ok(await _service.UpdateAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var itemId = await _service.DeleteAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("approved/{id:guid}")]
    public async Task<IActionResult> ApprovedAsync(Guid id)
    {
        var itemId = await _service.ApprovedAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("rejected/{id:guid}")]
    public async Task<IActionResult> RejectedAsync(Guid id)
    {
        var itemId = await _service.RejectedAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("createTable/{id:guid}")]
    public async Task<IActionResult> CreateTableAsync(Guid id)
    {
        var item = await _sqlService.CreateTableAsync(id);
        return Ok(item);
    }
}