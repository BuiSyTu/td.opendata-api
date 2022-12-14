using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class DatasetListFilter : PaginationFilter
{
    public Guid? LicenseId { get; set; }
    public Guid? OrganizationId { get; set; }
    public Guid? DataTypeId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ProviderTypeId { get; set; }
    public int? ApproveState { get; set; }
    public bool? Visibility { get; set; }
    public string? DataTypeCode { get; set; }
    public string? Author { get; set; }
    public string? OfficeCode { get; set; }
    public bool? IsPortal { get; set; }
    public bool? IsOffice { get; set; }
}