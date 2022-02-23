using Newtonsoft.Json.Linq;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public static class CommonService
{
    public static async Task<Stream?> ReadFromUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        using HttpClient client = new();
        byte[]? content = await client.GetByteArrayAsync(url);
        return new MemoryStream(content);
    }

    public static PreviewData ToPreviewData(this string jsonString)
    {
        PreviewData previewData = new();
        previewData.Data = JObject.Parse(jsonString);

        MetadataCollection metadataCollection = new();
        foreach (JProperty property in previewData.Data.Properties())
        {
            metadataCollection.Add(new Metadata
            {
                Data = property.Name,
                Example = property.Value.ToString(),
                Type = property.Value.Type.ToString(),
                IsDisplay = true,
                Title = property.Name
            });
        }

        previewData.Metadata = metadataCollection;
        return previewData;
    }
}
