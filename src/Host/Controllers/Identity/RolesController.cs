using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TD.OpenData.WebApi.Host.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
[ApiVersionNeutral]
[ApiConventionType(typeof(FSHApiConventions))]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("all")]
    [MustHavePermission(PermissionConstants.Roles.ListAll)]
    public async Task<ActionResult<Result<List<RoleDto>>>> GetListAsync()
    {
        var roles = await _roleService.GetListAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    [MustHavePermission(PermissionConstants.Roles.View)]
    public async Task<ActionResult<Result<RoleDto>>> GetByIdAsync(string id)
    {
        var roles = await _roleService.GetByIdAsync(id);
        return Ok(roles);
    }

    [HttpGet("{id}/permissions")]
    public async Task<ActionResult<Result<List<PermissionDto>>>> GetPermissionsAsync(string id)
    {
        var roles = await _roleService.GetPermissionsAsync(id);
        return Ok(roles);
    }

    [HttpPut("{id}/permissions")]
    public async Task<ActionResult<Result<string>>> UpdatePermissionsAsync(string id, List<UpdatePermissionsRequest> request)
    {
        var roles = await _roleService.UpdatePermissionsAsync(id, request);
        return Ok(roles);
    }

    [HttpPost]
    [MustHavePermission(PermissionConstants.Roles.Register)]
    public async Task<ActionResult<Result<string>>> RegisterRoleAsync(RoleRequest request)
    {
        var response = await _roleService.RegisterRoleAsync(request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(PermissionConstants.Roles.Remove)]
    public async Task<ActionResult<Result<string>>> DeleteAsync(string id)
    {
        var response = await _roleService.DeleteAsync(id);
        return Ok(response);
    }
}