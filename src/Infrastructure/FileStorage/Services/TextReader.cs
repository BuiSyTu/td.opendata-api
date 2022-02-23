using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

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
