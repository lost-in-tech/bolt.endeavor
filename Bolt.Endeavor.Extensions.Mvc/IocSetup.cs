using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class IocSetup
{
    public static IServiceCollection AddRequestBus(this IServiceCollection services, 
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
        services.TryAddSingleton<IHttpContextWrapper,HttpContextWrapper>();
        services.TryAddSingleton<ILogScopeProvider,DefaultLogScopeProvider>();
        services.TryAddSingleton<ITraceIdProvider,TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider,CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider,CurrentUserProvider>();

        return services;
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