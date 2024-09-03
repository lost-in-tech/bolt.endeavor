using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class IocSetup
{
    public static IServiceCollection AddRequestBus(this IServiceCollection services)
    {
        services.AddRequestBus(new RequestBusOptions
        {
            UseEmptyTenantProvider = false,
            UseEmptyCurrentUserProvider = false,
            UseEmptyTraceIdProvider = false
        });

        services.AddLogging();
        services.AddHttpContextAccessor();
        services.TryAddSingleton<ITraceIdProvider,TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider,CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider,CurrentUserProvider>();

        return services;
    }

    public static IApplicationBuilder UseDefaultLogScopes(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogScopeMiddleware>();

        return app;
    }
}