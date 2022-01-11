namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class DatasetDBConfigDto : IDto
{
    public Guid Id { get; set; }
    public string? DBProvider { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? DataTable { get; set; }
    public string? TableName { get; set; }
}