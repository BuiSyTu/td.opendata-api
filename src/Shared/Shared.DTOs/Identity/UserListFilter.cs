using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Shared.DTOs.Identity;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}