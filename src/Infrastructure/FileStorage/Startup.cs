using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Services;
using TextReader = TD.OpenData.WebApi.Infrastructure.FileStorage.Services.TextReader;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage;

internal static class Startup
{
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app) =>
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            RequestPath = new PathString("/Files")
        });
}