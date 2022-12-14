namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class DatasetFileConfigDto : IDto
{
    public Guid Id { get; set; }
    public string? FileType { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? SheetName { get; set; }
}