using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TD.OpenData.WebApi.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(Assembly.GetExecutingAssembly());
}