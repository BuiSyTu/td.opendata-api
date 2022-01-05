namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateLicenseRequest : IMustBeValid
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool? IsActive { get; set; }
}