using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class SyncHistoryListFilter : PaginationFilter
{
    public Guid? DatasetId { get; set; }
    public string? DataTypeCode { get; set; }
    public string? OfficeCode { get; set; }
    public bool? IsPortal { get; set; }
}