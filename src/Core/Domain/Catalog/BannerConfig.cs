using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class BannerConfig : AuditableEntity
{
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }


    public BannerConfig Update(UpdateBannerConfigRequest request)
    {
        if (request.Line1 != null && !Line1.NullToString().Equals(request.Line1)) Line1 = request.Line1;
        if (request.Line2 != null && !Line2.NullToString().Equals(request.Line2)) Line2 = request.Line2;
        return this;
    }
}