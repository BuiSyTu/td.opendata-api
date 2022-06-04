using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetAPIConfig : AuditableEntity, IMustHaveTenant
{
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? Headers { get; set; }
    public string? Data { get; set; }
    public string? DataKey { get; set; }
    public Guid? DatasetId { get; set; }
    public virtual Dataset Dataset { get; set; } = default!;

    public string? Tenant { get; set; }

    public DatasetAPIConfig Update(string? method, string? url, string? headers, string? data)
    {
        if (method != null && !Method.NullToString().Equals(method)) Method = method;
        if (url != null && !Url.NullToString().Equals(url)) Url = url;
        if (headers != null && !Headers.NullToString().Equals(headers)) Headers = headers;
        if (data != null && !Data.NullToString().Equals(data)) Data = data;

        return this;
    }

    public DatasetAPIConfig Update(DatasetAPIConfigDto request)
    {
        if (request.Method != null && !Method.NullToString().Equals(request.Method)) Method = request.Method;
        if (request.Url != null && !Url.NullToString().Equals(request.Url)) Url = request.Url;
        if (request.Headers != null && !Headers.NullToString().Equals(request.Headers)) Headers = request.Headers;
        if (request.Data != null && !Data.NullToString().Equals(request.Data)) Data = request.Data;

        return this;
    }

    public DatasetAPIConfig Update(CreateDatasetAPIConfigRequest request)
    {
        if (request.Method != null && !Method.NullToString().Equals(request.Method)) Method = request.Method;
        if (request.Url != null && !Url.NullToString().Equals(request.Url)) Url = request.Url;
        if (request.Headers != null && !Headers.NullToString().Equals(request.Headers)) Headers = request.Headers;
        if (request.Data != null && !Data.NullToString().Equals(request.Data)) Data = request.Data;

        return this;
    }
}