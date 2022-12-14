using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class ProductsController : BaseController
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync([FromQuery] ProductListFilter filter)
    {
        var products = await _service.SearchAsync(filter);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var product = await _service.GetProductDetailsAsync(id);
        return Ok(product);
    }

    [HttpPost("search")]
    [MustHavePermission(PermissionConstants.Products.Search)]
    public async Task<IActionResult> SearchAsync(ProductListFilter filter)
    {
        var products = await _service.SearchAsync(filter);
        return Ok(products);
    }

    [HttpGet("dapper")]
    [MustHavePermission(PermissionConstants.Products.View)]
    public async Task<IActionResult> GetDapperAsync(Guid id)
    {
        var products = await _service.GetByIdUsingDapperAsync(id);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request)
    {
        return Ok(await _service.CreateProductAsync(request));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(PermissionConstants.Products.Update)]
    public async Task<IActionResult> UpdateAsync(UpdateProductRequest request, Guid id)
    {
        return Ok(await _service.UpdateProductAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(PermissionConstants.Products.Remove)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var productId = await _service.DeleteProductAsync(id);
        return Ok(productId);
    }
}