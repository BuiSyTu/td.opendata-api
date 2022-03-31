using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

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

    public static PreviewData ToPreviewData(this string jsonString, string? sheetName = null)
    {
        PreviewData previewData = new();
        JArray? jArray = new JArray();

        if (string.IsNullOrEmpty(sheetName))
        {
            jArray = JArray.Parse(jsonString);
        }
        else
        {
            var jobject = JObject.Parse(jsonString);
            jArray = (JArray?)jobject[sheetName];
        }

        List<Dictionary<string, object>> ds = new();
        foreach (var item in jArray)
        {
            var jObject = (JObject)item;
            Dictionary<string, object> d = new();

            foreach (JProperty property in jObject.Properties())
            {
                if (property.Name.ToLower() != "id")
                {
                    d.Add(property.Name, property.Value.ToString());
                }
            }

            ds.Add(d);
        }

        previewData.Data = ds;

        MetadataCollection metadataCollection = new();
        foreach (JProperty property in jArray.Children<JObject>().First().Properties())
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

    public static MetadataCollection ToMetadataCollection(this string jsonString)
    {
        var data = JObject.Parse(jsonString);

        MetadataCollection metadataCollection = new();
        foreach (JProperty property in data.Properties())
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

        return metadataCollection;
    }
}
