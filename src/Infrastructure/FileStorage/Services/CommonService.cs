using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public static class CommonService
{
    public static async Task<Stream?> ReadFromUrl(string? url)
    {
        if (url is null) return null;

        using HttpClient client = new();
        byte[]? content = await client.GetByteArrayAsync(url);
        return new MemoryStream(content);
    }
}
