namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateDatasetAPIConfigRequest : IMustBeValid
{
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? Headers { get; set; }
    public string? Data { get; set; }
    public string? DataKey { get; set; }
}