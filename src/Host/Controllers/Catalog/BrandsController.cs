using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class BrandsController : BaseController
{
    private readonly IBrandService _service;

    public BrandsController(IBrandService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsybc([FromQuery] BrandListFilter filter)
    {
        var brands = await _service.SearchAsync(filter);
        return Ok(brands);
    }

    [HttpPost("search")]
    [OpenApiOperation("Search Brands using available Filters.", "")]
    public async Task<IActionResult> SearchAsync(BrandListFilter filter)
    {
        var brands = await _service.SearchAsync(filter);
        return Ok(brands);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateBrandRequest request)
    {
        return Ok(await _service.CreateBrandAsync(request));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(UpdateBrandRequest request, Guid id)
    {
        return Ok(await _service.UpdateBrandAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(PermissionConstants.Brands.Remove)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var brandId = await _service.DeleteBrandAsync(id);
        return Ok(brandId);
    }

    [HttpPost("generate-random")]
    public async Task<IActionResult> GenerateRandomAsync(GenerateRandomBrandRequest request)
    {
        var jobId = await _service.GenerateRandomBrandAsync(request);
        return Ok(jobId);
    }

    [HttpDelete("delete-random")]
    [ProducesResponseType(200)]
    [ProducesDefaultResponseType(typeof(ErrorResult))]
    public async Task<IActionResult> DeleteRandomAsync()
    {
        var jobId = await _service.DeleteRandomBrandAsync();
        return Ok(jobId);
    }
}