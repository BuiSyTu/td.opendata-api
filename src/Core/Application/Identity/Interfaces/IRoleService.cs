using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Identity;

namespace TD.OpenData.WebApi.Application.Identity.Interfaces;

public interface IRoleService : ITransientService
{
    Task<Result<List<RoleDto>>> GetListAsync();

    Task<Result<List<PermissionDto>>> GetPermissionsAsync(string id);

    Task<int> GetCountAsync();

    Task<Result<RoleDto>> GetByIdAsync(string id);

    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<Result<string>> RegisterRoleAsync(RoleRequest request);

    Task<Result<string>> DeleteAsync(string id);

    Task<Result<List<RoleDto>>> GetUserRolesAsync(string userId);

    Task<Result<string>> UpdatePermissionsAsync(string id, List<UpdatePermissionsRequest> request);
}