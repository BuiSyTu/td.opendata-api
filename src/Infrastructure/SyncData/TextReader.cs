using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using TD.OpenData.WebApi.Application.SyncData.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.SyncData;

public class TextReader : ITextReader
{
    public async Task<PreviewData> GetData(string? url)
    {
        var stream = await CommonService.ReadFromUrl(url);
        return GetData(stream);
    }

    public PreviewData GetData(Stream? stream)
    {
        StreamReader streamReader = new(stream);
        string jsonString = streamReader.ReadToEnd();

        return jsonString.ToPreviewData();
    }
}
