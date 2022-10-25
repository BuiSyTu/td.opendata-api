using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog.SyncHistory;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TD.OpenData.WebApi.Application.Forward;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.SyncData.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Filters;
using Microsoft.AspNetCore.Authorization;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class DatasetsController : BaseController
{
    private readonly IDatasetService _service;

    public DatasetsController(IDatasetService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}/data")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRawAsync(Guid id, string? orderBy = null, int skip = 0, int top = 20)
    {
        object? items = await _service.GetDataAsync(id, orderBy, skip, top);
        return Ok(items);
    }

    [HttpGet("{id:guid}/config")]
    [AllowAnonymous]
    public async Task<IActionResult> GetConfigAsync(Guid id)
    {
        object? items = await _service.GetRawAsync(id);
        return Ok(items);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync([FromQuery] DatasetListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var product = await _service.GetDetailsAsync(id);
        return Ok(product);
    }

    [HttpGet("stats-by-organization")]
    [AllowAnonymous]
    public IActionResult GroupByOrganization()
    {
        object? result = _service.GroupByOrganization();
        return Ok(result);
    }

    [HttpGet("stats-by-category")]
    [AllowAnonymous]
    public IActionResult GroupByCategory()
    {
        object? result = _service.GroupByCategory();
        return Ok(result);
    }

    [HttpPost("search")]
    [OpenApiOperation("Danh sách dataset.", "")]
    public async Task<IActionResult> SearchAsync(DatasetListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync(CreateDatasetRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpPut("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateAsync(UpdateDatasetRequest request, Guid id)
    {
        return Ok(await _service.UpdateAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var itemId = await _service.DeleteAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("approved/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> ApprovedAsync(Guid id)
    {
        var itemId = await _service.ApprovedAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("rejected/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> RejectedAsync(Guid id)
    {
        var itemId = await _service.RejectedAsync(id);
        return Ok(itemId);
    }

    [HttpPatch("syncData/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> SyncAsync(Guid id)
    {
        await _service.SyncDataAsync(id);
        return Ok(id);
    }
}