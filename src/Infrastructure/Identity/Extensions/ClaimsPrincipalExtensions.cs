using System.Security.Claims;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.AzureAd;

namespace TD.OpenData.WebApi.Infrastructure.Identity.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetPermissions(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimConstants.Permissions);
    public static string? GetUserPositionCode(this ClaimsPrincipal principal) =>
    principal.FindFirstValue(ClaimConstants.UserPositionCode);
    public static string? GetUserOffice(this ClaimsPrincipal principal) =>
    principal.FindFirstValue(ClaimConstants.UserOffice);
    public static string? GetAreaCode(this ClaimsPrincipal principal) =>
    principal.FindFirstValue(ClaimConstants.AreaCode);
    public static string? GetUserName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string? GetUserEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email);

    public static string? GetTenant1(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimConstants.Tenant);

    public static string? GetTenant(this ClaimsPrincipal principal)
    {
       if (principal.FindFirstValue(ClaimConstants.Tenant) is string tenant)
        {
            return tenant;
        }

       return "root";
    }

    public static string? GetIssuer(this ClaimsPrincipal principal)
    {
        if (principal.FindFirstValue(OpenIdConnectClaimTypes.Issuer) is string issuer)
        {
            return issuer;
        }

        // Workaround to deal with missing "iss" claim. We search for the ObjectId claim instead and return the value of Issuer property of that Claim
        return principal.FindFirst(AzureADClaimTypes.ObjectId)?.Issuer;
    }
}