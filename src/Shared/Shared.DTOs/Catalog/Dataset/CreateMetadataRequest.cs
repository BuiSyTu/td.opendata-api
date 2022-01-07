namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateMetadataRequest : IMustBeValid
{
    public string? Data { get; set; }
    public string? Title { get; set; }
    public string? DataType { get; set; }
    public string? Description { get; set; }
    public bool? IsDisplay { get; set; }
}