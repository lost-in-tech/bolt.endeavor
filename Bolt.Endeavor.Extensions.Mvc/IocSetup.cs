using System.Reflection;
using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class IocSetup
{
    public static IServiceCollection AddRequestBusForMvc(this IServiceCollection services,
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
        services.TryAddSingleton<ILogScopeProvider, DefaultLogScopeProvider>();
        services.TryAddSingleton<ITraceIdProvider, TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider, CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider, CurrentUserProvider>();
        services.TryAddSingleton<IExceptionHandler, GlobalErrorHandler>();

        if (options.UseDefaultGlobalErrorHandler)
        {
            services.AddExceptionHandler<GlobalErrorHandler>();
        }

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
        var types = assembly.GetExportedTypes()
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
    public Assembly[] AssembliesToScan { get; init; } = [Assembly.GetEntryAssembly()];
    public bool UseDefaultGlobalErrorHandler { get; init; } = true;

    public string TraceIdHeaderName { get; init; } = "x-trace-id";
    public string ConsumerIdHeaderName { get; init; } = "x-app-id";
    public string TenantHeaderName { get; init; } = "x-tenant";
    public string TenantRouteName { get; init; } = "tenant";
    public string TenantQueryName { get; init; } = "tenant";

    public string TraceIdLogKey { get; init; } = "traceId";
    public string TenantLogKey { get; init; } = "tenant";
    public string UserIdLogKey { get; init; } = "userId";
    public string AppIdLogKey { get; init; } = "appId";
    public string ConsumerIdLogKey { get; init; } = "consumerId";
}