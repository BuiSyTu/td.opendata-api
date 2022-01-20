namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class DatasetDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string? Tags { get; set; }
    public bool? Visibility { get; set; }
    public int? State { get; set; }
    public Guid? OrganizationId { get; set; }
    public OrganizationDto? Organization { get; set; }
}