using TD.OpenData.WebApi.Shared.DTOs.FileStorage;

namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class UpdateCategoryRequest : IMustBeValid
{
    public Guid? ParentId { get; private set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string? Icon { get; set; }
    public int? Order { get; set; }
    public bool? IsActive { get; set; }
    public FileUploadRequest? Image { get; set; }
}