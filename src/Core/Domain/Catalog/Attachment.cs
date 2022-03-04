using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class Attachment : AuditableEntity, IMustHaveTenant
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public string? Tenant { get; set; }

    public Attachment()
    {
    }
}