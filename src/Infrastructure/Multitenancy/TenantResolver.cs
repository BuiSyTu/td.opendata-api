using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Http;

namespace TD.OpenData.WebApi.Infrastructure.Multitenancy;

public static class TenantResolver
{
    public static string? Resolver(HttpContext context)
    {
        string? tenantId = ResolveFromUserAuth(context);
        if (!string.IsNullOrEmpty(tenantId))
        {
            return tenantId;
        }

        tenantId = ResolveFromHeader(context);
        if (!string.IsNullOrEmpty(tenantId))
        {
            return tenantId;
        }

        tenantId = ResolveFromQuery(context);
        if (!string.IsNullOrEmpty(tenantId))
        {
            return tenantId;
        }

        return default;
    }

    private static string? ResolveFromUserAuth(HttpContext context) =>
        context.User.GetTenant();

    private static string? ResolveFromHeader(HttpContext context) =>
        context.Request.Headers.TryGetValue(HeaderConstants.Tenant, out var tenantFromHeader) ? (string)tenantFromHeader : default;

    private static string? ResolveFromQuery(HttpContext context) =>
        context.Request.Query.TryGetValue(QueryConstants.Tenant, out var tenantFromQueryString) ? (string)tenantFromQueryString : default;
}