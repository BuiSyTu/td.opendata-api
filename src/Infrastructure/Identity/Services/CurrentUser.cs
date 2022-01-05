using System.Security.Claims;
using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Infrastructure.Identity.Extensions;

namespace TD.OpenData.WebApi.Infrastructure.Identity.Services;

public class CurrentUser : ICurrentUser
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private string _userId = string.Empty;

    public string GetUserName() =>
        IsAuthenticated() ? (_user?.GetUserName() ?? string.Empty) : _userId;

    public string? GetUserEmail() =>
        IsAuthenticated() ? _user?.GetUserEmail() : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) ?? false;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetTenant() =>
        IsAuthenticated() ? _user?.GetTenant() : "root";

    public void SetUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetUserJob(string userId)
    {
        if (_userId != string.Empty)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = userId;
        }
    }

    public string? GetPermissions() =>
        IsAuthenticated() ? _user?.GetPermissions() : string.Empty;

    public string? GetUserPositionCode() =>
        IsAuthenticated() ? _user?.GetUserPositionCode() : string.Empty;

    public string? GetUserOffice() =>
        IsAuthenticated() ? _user?.GetUserOffice() : string.Empty;

    public string? GetAreaCode() =>
        IsAuthenticated() ? _user?.GetAreaCode() : string.Empty;
}