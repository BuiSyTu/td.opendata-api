using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<Result<ProductDetailsDto>>> GetAsync([FromQuery] ProductListFilter filter)
    {
        var products = await _service.SearchAsync(filter);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<ProductDetailsDto>>> GetByIdAsync(Guid id)
    {
        var product = await _service.GetProductDetailsAsync(id);
        return Ok(product);
    }

    [HttpPost("search")]
    [MustHavePermission(PermissionConstants.Products.Search)]
    public async Task<ActionResult<PaginatedResult<ProductDto>>> SearchAsync(ProductListFilter filter)
    {
        var products = await _service.SearchAsync(filter);
        return Ok(products);
    }

    [HttpGet("dapper")]
    [MustHavePermission(PermissionConstants.Products.View)]
    public async Task<ActionResult<Result<ProductDto>>> GetDapperAsync(Guid id)
    {
        var products = await _service.GetByIdUsingDapperAsync(id);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> CreateAsync(CreateProductRequest request)
    {
        return Ok(await _service.CreateProductAsync(request));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(PermissionConstants.Products.Update)]
    public async Task<ActionResult<Result<Guid>>> UpdateAsync(UpdateProductRequest request, Guid id)
    {
        return Ok(await _service.UpdateProductAsync(request, id));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(PermissionConstants.Products.Remove)]
    public async Task<ActionResult<Result<Guid>>> DeleteAsync(Guid id)
    {
        var productId = await _service.DeleteProductAsync(id);
        return Ok(productId);
    }
}