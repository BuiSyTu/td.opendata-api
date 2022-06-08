using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class SyncHistory : AuditableEntity
{
    public DateTime? SyncTime { get; set; }
    public Guid? DatasetId { get; set; }
    public virtual Dataset Dataset { get; set; } = default!;
}