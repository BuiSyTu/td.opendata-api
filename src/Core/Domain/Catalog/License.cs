using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class License : AuditableEntity, IMustHaveTenant
{
    public string? Name { get;  set; }
    public string? Description { get;  set; }
    public string? Code { get; set; }
    public bool? IsActive { get;  set; }
    public string? Tenant { get; set; }

    public virtual ICollection<Dataset>? Datasets { get; set; }

    public License Update(string? name, string? description, string? code, bool? isActive)
    {
        if (name != null && !Name.NullToString().Equals(name)) Name = name;
        if (description != null && !Description.NullToString().Equals(description)) Description = description;
        if (code != null && !Code.NullToString().Equals(code)) Code = code;
        if (isActive != null && IsActive != isActive) IsActive = isActive;
        return this;
    }
}