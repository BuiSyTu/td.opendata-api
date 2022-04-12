namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateDatasetFileConfigRequest : IMustBeValid
{
    public string? FileType { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? SheetName { get; set; }
}