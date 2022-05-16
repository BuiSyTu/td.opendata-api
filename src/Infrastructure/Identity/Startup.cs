using System.Net;
using System.Security.Claims;
using System.Text;
using TD.OpenData.WebApi.Application.Identity.Exceptions;
using TD.OpenData.WebApi.Infrastructure.Identity.AzureAd;
using TD.OpenData.WebApi.Infrastructure.Identity.Models;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace TD.OpenData.WebApi.Infrastructure.Identity;

internal static class Startup
{
    internal static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services.AddScoped<CurrentUserMiddleware>();

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();

    internal static IServiceCollection AddPermissions(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    internal static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
        {
            services.AddAzureAdAuthentication(config);
        }
        else
        {
            services.AddJwtAuthentication(config);
        }

        return services;
    }

    private static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var logger = Log.ForContext(typeof(AzureAdJwtBearerEvents));

        services.AddAuthorization();

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddMicrosoftIdentityWebApi(
                jwtOptions => jwtOptions.Events = new AzureAdJwtBearerEvents(logger, config),
                msIdentityOptions => config.GetSection("SecuritySettings:AzureAd").Bind(msIdentityOptions));

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection($"SecuritySettings:{nameof(JwtSettings)}"));
        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(JwtSettings)}").Get<JwtSettings>();

        if (string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);
        _ = services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                bearer.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            throw new IdentityException("Authentication Failed.", statusCode: HttpStatusCode.Unauthorized);
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = _ => throw new IdentityException("You are not authorized to access this resource.", statusCode: HttpStatusCode.Forbidden),
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.ContainsKey("TDAuthorization"))
                        {
                            string bearerToken = context.Request.Headers["TDAuthorization"].ElementAt(0);
                            context.Token = bearerToken.StartsWith("Bearer ") ? bearerToken[7..] : bearerToken;
                        }

                        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                        }

                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(x =>
        {
            x.AddPolicy("QTDanhMuc", policy => policy.RequireClaim("permissions", "QTDanhMuc"));
            x.AddPolicy("QTDanhMucTTHC", policy => policy.RequireClaim("permissions", "QTDanhMucTTHC"));

            x.AddPolicy("DataHTThuThap", policy => policy.RequireClaim("permissions", "DataHT.ThuThap"));
            x.AddPolicy("DataHTDuyet", policy => policy.RequireClaim("permissions", "DataHT.Duyet"));
            x.AddPolicy("DataHTChiaSe", policy => policy.RequireClaim("permissions", "DataHT.ChiaSe"));
            x.AddPolicy("DataHTKhaiThac", policy => policy.RequireClaim("permissions", "DataHT.KhaiThac"));

            x.AddPolicy("DataDVThuThap", policy => policy.RequireClaim("permissions", "DataDV.ThuThap"));
            x.AddPolicy("DataDVDuyet", policy => policy.RequireClaim("permissions", "DataDV.Duyet"));
            x.AddPolicy("DataDVKhaiThac", policy => policy.RequireClaim("permissions", "DataDV.KhaiThac"));
            x.AddPolicy("DataDVChiaSe", policy => policy.RequireClaim("permissions", "DataDV.ChiaSe"));

            x.AddPolicy("CongCanBoQTDanhMuc", policy => policy.RequireClaim("permissions", "CongCanBo.QTDanhMuc"));
            x.AddPolicy("CongCanBoTraCuuDL", policy => policy.RequireClaim("permissions", "CongCanBo.TraCuuDL"));
            x.AddPolicy("CongMoQTDanhMuc", policy => policy.RequireClaim("permissions", "CongMo.QTDanhMuc"));
            x.AddPolicy("CongMoQTTaiKhoan", policy => policy.RequireClaim("permissions", "CongMo.QTTaiKhoan"));
            x.AddPolicy("CongMoQTDuLieu", policy => policy.RequireClaim("permissions", "CongMo.QTDuLieu"));
        });
        return services;
    }
}