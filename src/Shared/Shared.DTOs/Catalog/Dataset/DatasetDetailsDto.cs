namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class DatasetDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string? Tags { get; set; }
    public bool? Visibility { get; set; }
    public int? ApproveState { get; set; }
    public bool? IsSynced { get; set; }

    public Guid? LicenseId { get; set; }
    public LicenseDto? License { get; set; }
    public string? Author { get; set; }
    public string? AuthorEmail { get; set; }
    public string? Maintainer { get; set; }
    public string? MaintainerEmail { get; set; }
    public Guid? OrganizationId { get; set; }
    public OrganizationDto? Organization { get; set; }
    public string? Resource { get; set; }
    public string? Metadata { get; set; }

    public string? TableName { get; set; }

    public int? View { get; set; }

    public Guid? DataTypeId { get; set; }
    public DataTypeDto? DataType { get; set; }
    public Guid? CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
    public Guid? ProviderTypeId { get; set; }
    public ProviderTypeDto? ProviderType { get; set; }

    public virtual DatasetAPIConfigDto? DatasetAPIConfig { get; set; } = default!;
    public virtual DatasetDBConfigDto? DatasetDBConfig { get; set; } = default!;
    public virtual DatasetFileConfigDto? DatasetFileConfig { get; set; } = default!;

}