using Bolt.Endeavor.Extensions.Bus.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Bus;
public static class IocSetup
{
    public static IServiceCollection AddRequestBus(this IServiceCollection services)
    {
        services.TryAddScoped<IBusContextFactory, BusContextFactory>();
        services.TryAddScoped<IRequestBus, RequestBus>();
        services.TryAdd(ServiceDescriptor.Transient(typeof(IProcessFilter<,>), typeof(ValidationProcessFilter<,>)));

        return services;
    }
}