using System.Reflection;
using Bolt.Endeavor.Extensions.Bus;
using Bolt.Endeavor.Extensions.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class IocSetup
{
    public static IServiceCollection AddRequestBusForMvc(this IServiceCollection services,
        IConfiguration configuration,
        RequestBusMvcOptions? options = null)
    {
        options ??= new RequestBusMvcOptions();

        services.AddSingleton<IDataKeySettings>(_ => options);
        services.AddRequestBus();

        services.TryAddSingleton<ITraceIdProvider, TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider, CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider, CurrentUserProvider>();

        services.AddLogging();
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IHttpContextWrapper, HttpContextWrapper>();
        services.TryAddSingleton<ITraceIdProvider, TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider, CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider, CurrentUserProvider>();
        services.TryAddSingleton<IExceptionHandler, GlobalErrorHandler>();

        services.TryAddSingleton<ITraceContextProvider, TraceContextProvider>();
        services.AddTracingFeatures(options.TracingOptions);
        
        if (options.UseDefaultGlobalErrorHandler)
        {
            services.AddExceptionHandler<GlobalErrorHandler>();
        }

        services.AddHttpClient();
        services.AddEndpoints(options.AssembliesToScan);
        services.ScanAndConfigure(configuration, options.AssembliesToScan);

        if (options.AutoRegister)
        {
            foreach (var assembly in options.AssembliesToScan)
            {
                AddServices(
                    services,
                    assembly,
                    [
                        typeof(IRequestValidator<>),
                        typeof(IRequestHandler<,>),
                        typeof(IProcessFilter<,>)
                    ]);
            }
        }

        return services;
    }

    private static void AddServices(IServiceCollection services, Assembly assembly, IEnumerable<Type> openGenericTypes)
    {
        var types = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces(), (type, i) => new { ImplementationType = type, InterfaceType = i })
            .Where(t => openGenericTypes.Any(openGenericType => IsMatch(t.InterfaceType, openGenericType)))
            .ToList();

        foreach (var typePair in types)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient(typePair.InterfaceType, typePair.ImplementationType));
        }
    }

    private static bool IsMatch(Type interfaceType, Type openGenericType)
    {
        if (openGenericType.IsGenericTypeDefinition)
        {
            return interfaceType.IsGenericType &&
                   interfaceType.GetGenericTypeDefinition() == openGenericType;
        }
        else
        {
            return interfaceType == openGenericType;
        }
    }

    /// <summary>
    /// Provide default logging data to your logs e.g traceId, tenant, userId 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDefaultLogScopes(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogScopeMiddleware>();

        return app;
    }
}

public record RequestBusMvcOptions : IDataKeySettings
{
    public bool AutoRegister { get; init; } = true;
    public Assembly[] AssembliesToScan { get; init; } = [Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()];
    public bool UseDefaultGlobalErrorHandler { get; init; } = true;

    public string TenantRouteName { get; init; } = "tenant";
    public string TenantQueryName { get; init; } = "tenant";
    
    public TracingIocSetupOptions? TracingOptions { get; init; }
}