namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateBrandRequest : CreatedRequest, IMustBeValid
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}