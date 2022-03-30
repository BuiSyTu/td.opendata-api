using TD.OpenData.WebApi.Infrastructure.Caching;
using TD.OpenData.WebApi.Infrastructure.Common;
using TD.OpenData.WebApi.Infrastructure.Cors;
using TD.OpenData.WebApi.Infrastructure.FileStorage;
using TD.OpenData.WebApi.Infrastructure.Hangfire;
using TD.OpenData.WebApi.Infrastructure.Identity;
using TD.OpenData.WebApi.Infrastructure.Localization;
using TD.OpenData.WebApi.Infrastructure.Mailing;
using TD.OpenData.WebApi.Infrastructure.Mapping;
using TD.OpenData.WebApi.Infrastructure.Middleware;
using TD.OpenData.WebApi.Infrastructure.Multitenancy;
using TD.OpenData.WebApi.Infrastructure.Notifications;
using TD.OpenData.WebApi.Infrastructure.SecurityHeaders;
using TD.OpenData.WebApi.Infrastructure.Seeding;
using TD.OpenData.WebApi.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.OpenData.WebApi.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        return services
            .AddApiVersioning()
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddCurrentUser()
            .AddCurrentTenant()
            .AddExceptionMiddleware()
            .AddHangfire(config)
            .AddHealthCheck()
            .AddIdentity(config)
            .AddLocalization(config)
            .AddMailing(config)
            .AddNotifications(config)
            .AddPermissions()
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddSeeders()
            .AddServices()
            .AddSwaggerDocumentation(config)
            .AddMultitenancy(config); // Multitency needs to be last as this one also creates and/or migrates the database(s).
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder, IConfiguration config) =>
        appBuilder
            .UseLocalization(config)
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseLocalization(config)
            .UseRouting()
            .UseCorsPolicy()

            // .UseAuthentication()
            .UseCurrentUser()
            .UseCurrentTenant()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseEndpoints(endpoints =>
            {
                // endpoints.MapControllers().RequireAuthorization();
                endpoints.MapControllers();
                endpoints.MapHealthCheck();
                endpoints.MapNotifications();
            })
            .UseSwaggerDocumentation(config);

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health").RequireAuthorization();
}