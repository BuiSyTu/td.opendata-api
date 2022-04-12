using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class Tag : AuditableEntity, IMustHaveTenant
{
    public string? Name { get;  set; }

    public int View { get; set; }

    public string? Tenant { get; set; }

    public Tag()
    {
    }

    public Tag Update(string? name)
    {
        if (name != null && !Name.NullToString().Equals(name)) Name = name;
        return this;
    }
}