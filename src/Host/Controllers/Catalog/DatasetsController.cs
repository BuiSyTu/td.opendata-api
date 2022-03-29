using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class DatasetsController : BaseController
{
    private readonly IDatasetService _service;

    public DatasetsController(IDatasetService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<DatasetDto>>> GetAsync([FromQuery] DatasetListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<DatasetDetailsDto>>> GetByIdAsync(Guid id)
    {
        var product = await _service.GetDetailsAsync(id);
        return Ok(product);
    }

    [HttpGet("{id:guid}/data")]
    public async Task<ActionResult<Result<DatasetDetailsDto>>> GetDataByIdAsync(Guid id)
    {
        var product = await _service.GetDetailsAsync(id);
        return Ok(product);
    }

    [HttpPost("search")]
    [OpenApiOperation("Danh sách dataset.", "")]
    public async Task<ActionResult<PaginatedResult<DatasetDto>>> SearchAsync(DatasetListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> CreateAsync(CreateDatasetRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<Guid>>> UpdateAsync(UpdateDatasetRequest request, Guid id)
    {
        return Ok(await _service.UpdateAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<Guid>>> DeleteAsync(Guid id)
    {
        var itemId = await _service.DeleteAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("approved/{id:guid}")]
    public async Task<ActionResult<Result<Guid>>> ApprovedAsync(Guid id)
    {
        var itemId = await _service.ApprovedAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("rejected/{id:guid}")]
    public async Task<ActionResult<Result<Guid>>> RejectedAsync(Guid id)
    {
        var itemId = await _service.RejectedAsync(id);
        return Ok(itemId);
    }
}