using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class Dataset : AuditableEntity, IMustHaveTenant
{
    public string? Name { get;  set; }
    public string? Title { get; set; }
    public string? Description { get;  set; }
    public string? Code { get; set; }
    public string? Tags { get; set; }
    public int? State { get;  set; }
    public bool? Visibility { get; set; }
    public Guid? LicenseId { get; set; }
    public virtual License License { get; set; } = default!;

    public string? Author { get; set; }
    public string? AuthorEmail { get; set; }
    public string? Maintainer { get; set; }
    public string? MaintainerEmail { get; set; }
    public Guid? OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = default!;

    public string? Resource { get; set; }
    public Guid? DataTypeId { get; set; }
    public virtual DataType DataType { get; set; } = default!;

    public Guid? CategoryId { get; set; }
    public virtual Category Category { get; set; } = default!;

    public Guid? ProviderTypeId { get; set; }
    public virtual ProviderType ProviderType { get; set; } = default!;


    public virtual ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();
    public virtual ICollection<Metadata> Metadatas { get; set; } = new List<Metadata>();
    public virtual DatasetAPIConfig DatasetAPIConfig { get; set; } = default!;
    public virtual DatasetFileConfig DatasetFileConfig { get; set; } = default!;
    public virtual DatasetDBConfig DatasetDBConfig { get; set; } = default!;


    public string? Tenant { get; set; }

    public Dataset()
    {
    }

    public Dataset Update(string? name, string? title, string? description, string? code, string? tags, int? state, bool? visibility, Guid? licenseId, string? author, string? authorEmail, string? maintainer, string? maintainerEmail, Guid? organizationId, string? resource, Guid? dataTypeId, Guid? categoryId, Guid? providerTypeId)
    {
        if (name != null && !Name.NullToString().Equals(name)) Name = name;
        if (title != null && !Title.NullToString().Equals(title)) Title = title;
        if (description != null && !Description.NullToString().Equals(description)) Description = description;
        if (code != null && !Code.NullToString().Equals(code)) Code = code;
        if (tags != null && !Tags.NullToString().Equals(tags)) Tags = tags;
        if (state != null && !State.NullToString().Equals(state)) State = state;
        if (visibility != null && Visibility != visibility) Visibility = visibility;

        if (licenseId != Guid.Empty && !LicenseId.NullToString().Equals(licenseId)) LicenseId = licenseId;
        if (organizationId != Guid.Empty && !OrganizationId.NullToString().Equals(organizationId)) OrganizationId = organizationId;
        if (dataTypeId != Guid.Empty && !DataTypeId.NullToString().Equals(dataTypeId)) DataTypeId = dataTypeId;
        if (categoryId != Guid.Empty && !CategoryId.NullToString().Equals(categoryId)) CategoryId = categoryId;
        if (providerTypeId != Guid.Empty && !ProviderTypeId.NullToString().Equals(providerTypeId)) ProviderTypeId = providerTypeId;

        if (author != null && !Author.NullToString().Equals(author)) Author = author;
        if (authorEmail != null && !AuthorEmail.NullToString().Equals(authorEmail)) AuthorEmail = authorEmail;
        if (maintainer != null && !Maintainer.NullToString().Equals(maintainer)) Maintainer = maintainer;
        if (maintainerEmail != null && !MaintainerEmail.NullToString().Equals(maintainerEmail)) MaintainerEmail = maintainerEmail;
        if (resource != null && !Resource.NullToString().Equals(resource)) Resource = resource;
        return this;
    }
}