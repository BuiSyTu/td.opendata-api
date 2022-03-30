using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class CategoriesController : BaseController
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] CategoryListFilter filter)
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
    [OpenApiOperation("Search Categorys using available Filters.", "")]
    public async Task<IActionResult> SearchAsync(CategoryListFilter filter)
    {
        var items = await _service.SearchAsync(filter);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(UpdateCategoryRequest request, Guid id)
    {
        return Ok(await _service.UpdateAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var itemId = await _service.DeleteAsync(id);
        return Ok(itemId);
    }
}