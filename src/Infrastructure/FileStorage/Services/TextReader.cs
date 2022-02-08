using Newtonsoft.Json;
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
        PreviewData previewData = new();
        StreamReader streamReader = new(stream);
        string jsonString = streamReader.ReadToEnd();
        previewData.Data = JsonConvert.DeserializeObject(jsonString);
        return previewData;
    }
}
