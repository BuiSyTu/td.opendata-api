using System.Security.Claims;
using TD.OpenData.WebApi.Application.Common.Interfaces;

namespace TD.OpenData.WebApi.Application.Identity.Interfaces;

public interface ICurrentUser : IScopedService
{
    string? Name { get; }

    string? GetUserName();

    string? GetUserEmail();

    string? GetTenant();

    string? GetPermissions();
    string? GetUserPositionCode();
    string? GetUserOffice();
    string? GetAreaCode();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();

    void SetUser(ClaimsPrincipal user);

    void SetUserJob(string userId);
}