using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace TD.OpenData.WebApi.Infrastructure.Identity.Permissions;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IRoleClaimsService _permissionService;

    public PermissionAuthorizationHandler(IRoleClaimsService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User?.GetUserName();
        if (userId is not null &&
            await _permissionService.HasPermissionAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}