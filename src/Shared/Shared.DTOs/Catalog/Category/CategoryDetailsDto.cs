namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CategoryDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool? IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? Icon { get; set; }
    public int? Order { get; set; }
    public Guid? ParentId { get; private set; }
    public virtual CategoryDto? Parent { get; set; }
}