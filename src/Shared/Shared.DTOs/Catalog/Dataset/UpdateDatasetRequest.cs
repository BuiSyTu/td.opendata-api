namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class UpdateDatasetRequest : IMustBeValid
{
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string? Tags { get; set; }
    public bool? Visibility { get; set; }
    public int? ApproveState { get; set; }
    public bool? IsSynced { get; set; }
    public Guid? LicenseId { get; set; }
    public string? Author { get; set; }
    public string? AuthorEmail { get; set; }
    public string? Maintainer { get; set; }
    public string? MaintainerEmail { get; set; }
    public Guid? OrganizationId { get; set; }
    public string? Resource { get; set; }
    public string? Metadata { get; set; }

    public string? TableName { get; set; }

    public Guid? DataTypeId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ProviderTypeId { get; set; }

    public string? Tenant { get; set; }

    public CreateDatasetAPIConfigRequest? APIConfig { get; set; }
    public CreateDatasetDBConfigRequest? DBConfig { get; set; }
    public CreateDatasetFileConfigRequest? FileConfig { get; set; }
}