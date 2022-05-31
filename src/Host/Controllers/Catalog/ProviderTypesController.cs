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
public class ProviderTypesController : BaseController
{
    private readonly IProviderTypeService _service;

    public ProviderTypesController(IProviderTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync([FromQuery] ProviderTypeListFilter filter)
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

    [HttpPost("search")]
    [OpenApiOperation("Search ProviderTypes using available Filters.", "")]
    public async Task<IActionResult> SearchAsync(ProviderTypeListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync(CreateProviderTypeRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpPut("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateAsync(UpdateProviderTypeRequest request, Guid id)
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
}