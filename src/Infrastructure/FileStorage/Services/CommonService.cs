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
}
