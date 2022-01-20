namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class DatasetAPIConfigDto : IDto
{
    public Guid Id { get; set; }
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? Headers { get; set; }
    public string? Data { get; set; }
    public string? DataKey { get; set; }
    public string? TableName { get; set; }
}