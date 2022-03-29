using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetAPIConfig : AuditableEntity, IMustHaveTenant
{
    public string? Method { get;  set; }
    public string? Url { get;  set; }
    public string? Headers { get; set; }
    public string? Data { get;  set; }
    public string? DataKey { get; set; }
    public string? TableName { get; set; }
    public Guid? DatasetId { get; set; }
    public virtual Dataset Dataset { get; set; } = default!;

    public string? Tenant { get; set; }

    public DatasetAPIConfig()
    {
    }

    public DatasetAPIConfig Update(string? method, string? url, string? headers, string? data, Guid? datasetId)
    {
        if (method != null && !Method.NullToString().Equals(method)) Method = method;
        if (url != null && !Url.NullToString().Equals(url)) Url = url;
        if (headers != null && !Headers.NullToString().Equals(headers)) Headers = headers;
        if (data != null && !Data.NullToString().Equals(data)) Data = data;
        if (datasetId != Guid.Empty && !DatasetId.NullToString().Equals(datasetId)) DatasetId = datasetId;

        return this;
    }
}