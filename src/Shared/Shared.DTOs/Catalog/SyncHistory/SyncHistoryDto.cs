namespace TD.OpenData.WebApi.Shared.DTOs.Catalog.SyncHistory;

public class SyncHistoryDto : IDto
{
    public Guid Id { get; set; }
    public DateTime? SyncTime { get; set; }
    public Guid? DatasetId { get; set; }
    public DatasetDto? Dataset { get; set; }
}