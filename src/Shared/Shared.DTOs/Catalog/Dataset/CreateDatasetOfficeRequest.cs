namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateDatasetOfficeRequest : IMustBeValid
{
    public string? OfficeCode { get; set; }
    public string? OfficeName { get; set; }
}