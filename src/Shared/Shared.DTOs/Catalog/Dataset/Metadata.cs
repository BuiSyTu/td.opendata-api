using Newtonsoft.Json;

namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class Metadata
{
    public string? Data { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? Example { get; set; }
    public bool IsDisplay { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static Metadata? Parse(string metadata)
    {
        return JsonConvert.DeserializeObject<Metadata>(metadata);
    }
}
