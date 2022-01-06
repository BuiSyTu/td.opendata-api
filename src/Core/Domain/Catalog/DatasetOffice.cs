using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetOffice : AuditableEntity, IMustHaveTenant
{
    public Guid? DatasetId { get;  set; }
    public virtual Dataset Dataset { get; set; } = default!;
    public string? OfficeCode { get;  set; }
    public string? Tenant { get; set; }

    public DatasetOffice()
    {
    }

    public DatasetOffice Update(in Guid datasetId, string? officeCode)
    {
        if (datasetId != Guid.Empty && !DatasetId.NullToString().Equals(datasetId)) DatasetId = datasetId;
        if (officeCode != null && !OfficeCode.NullToString().Equals(officeCode)) OfficeCode = officeCode;
        return this;
    }
}