using Microsoft.AspNetCore.Authorization;

namespace TD.OpenData.WebApi.Infrastructure.Identity.Permissions;

public class MustHavePermission : AuthorizeAttribute
{
    public MustHavePermission(string permission)
    {
        Policy = permission;
    }
}