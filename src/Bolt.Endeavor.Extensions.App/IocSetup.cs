using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.App;

public static class IocSetup
{
    public static IServiceCollection AddBoltEndeavorApp(this IServiceCollection services, IConfiguration configuration, BoltEndeavorAppOptions? options)
    {
        options ??= new BoltEndeavorAppOptions();

        var appName = options.AppName;
        if (string.IsNullOrWhiteSpace(appName))
        {
            appName = configuration["appName"] ?? configuration["app"] ?? configuration["appId"];
        }

        appName ??= Assembly.GetExecutingAssembly().GetName().Name;
        
        services.TryAddSingleton<IAppNameProvider>(_ => new AppNameProvider(appName));

        return services;
    }
}

public record BoltEndeavorAppOptions
{
    public string? AppName { get; init; }
}