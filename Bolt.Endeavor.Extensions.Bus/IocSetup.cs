using Bolt.Endeavor.Extensions.Bus.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Bus;
public static class IocSetup
{
    public static IServiceCollection AddRequestBus(this IServiceCollection services, RequestBusOptions? options = null)
    {
        options ??= new RequestBusOptions();

        if (options.UseEmptyTraceIdProvider)
        {
            services.TryAddSingleton<ITraceIdProvider, NullTraceIdProvider>();
        }

        if (options.UseEmptyTenantProvider)
        {
            services.TryAddSingleton<ICurrentTenantProvider, NullTenantNameProvider>();
        }

        if (options.UseEmptyCurrentUserProvider)
        {
            services.TryAddSingleton<ICurrentUserProvider, NullCurrentUserProvider>();
        }

        services.TryAddScoped<IBusContextFactory, BusContextFactory>();
        services.TryAddScoped<IRequestBus, RequestBus>();
        services.TryAdd(ServiceDescriptor.Transient(typeof(IProcessFilter<,>), typeof(ValidationProcessFilter<,>)));

        return services;
    }
}

public record RequestBusOptions
{
    /// <summary>
    /// A default tenant provider will be used which always return empty tenant.
    /// If you like to use this feature add your own impl in IOC of ICurrentTenantProvider and set the value to false.
    /// </summary>
    public bool UseEmptyTenantProvider { get; init; } = true;

    /// <summary>
    /// A default trace id provider will be used which always return empty string.
    /// If you like to use this feature add your own impl in IOC of ITraceIdProvider and set the value to false.
    /// </summary>
    public bool UseEmptyTraceIdProvider { get; init; } = true;

    /// <summary>
    /// A default CurrentUserProvider will be used which always return empty CurrentUser dto.
    /// If you like to use this feature add your own impl in IOC of ICurrentUserProvider and set the value to false.
    /// </summary>
    public bool UseEmptyCurrentUserProvider { get; init; } = true;
}
