using TD.OpenData.WebApi.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace TD.OpenData.WebApi.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser, IIdentityTenant
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? Tenant { get; set; }

    public string? ObjectId { get; set; }
}