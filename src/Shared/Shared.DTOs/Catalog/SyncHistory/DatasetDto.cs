namespace TD.OpenData.WebApi.Shared.DTOs.Catalog.SyncHistory;

public class DatasetDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string? Tags { get; set; }
    public bool? Visibility { get; set; }
    public int? ApproveState { get; set; }
    public bool? IsSynced { get; set; }
}