using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class SyncHistoriesController : BaseController
{
    private readonly ISyncHistoryService _service;

    public SyncHistoriesController(ISyncHistoryService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync([FromQuery] SyncHistoryListFilter filter)
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

    [HttpDelete("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var itemId = await _service.DeleteAsync(id);
        return Ok(itemId);
    }
}