using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class Metadata : AuditableEntity, IMustHaveTenant
{
    public string? DataType { get;  set; }
    public bool? IsDisplay { get;  set; }
    public string? Data { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? DatasetId { get; set; }
    public virtual Dataset Dataset { get; set; } = default!;

    public string? Tenant { get; set; }

    public Metadata()
    {
    }

    public Metadata Update(string? dataType, bool? isDisplay, string? data, string? title, string? description, Guid? datasetId)
    {
        if (dataType != null && !DataType.NullToString().Equals(dataType)) DataType = dataType;
        if (description != null && !Description.NullToString().Equals(description)) Description = description;
        if (data != null && !Data.NullToString().Equals(data)) Data = data;
        if (title != null && !Title.NullToString().Equals(title)) Title = title;
        if (isDisplay != null && IsDisplay != isDisplay) IsDisplay = isDisplay;

        if (datasetId != Guid.Empty && !DatasetId.NullToString().Equals(datasetId)) DatasetId = datasetId;

        return this;
    }
}