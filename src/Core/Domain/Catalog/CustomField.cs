using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class CustomField : AuditableEntity, IMustHaveTenant
{
    public Guid? DatasetId { get;  set; }
    public virtual Dataset Dataset { get; set; } = default!;

    public string? Key { get;  set; }
    public string? Value { get; set; }
    public string? Tenant { get; set; }

    public CustomField()
    {
    }

    public CustomField Update(in Guid datasetId, string? key, string? value)
    {
        if (datasetId != Guid.Empty && !DatasetId.NullToString().Equals(datasetId)) DatasetId = datasetId;

        if (key != null && !Key.NullToString().Equals(key)) Key = key;
        if (value != null && !Value.NullToString().Equals(value)) Value = value;
        
        return this;
    }
}