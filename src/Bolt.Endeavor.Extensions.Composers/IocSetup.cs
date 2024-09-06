using Bolt.Endeavor.Extensions.Bus;
using Bolt.Endeavor.Extensions.Composers.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Composers;

public static class IocSetup
{
    public static IServiceCollection AddResponseComposer(this IServiceCollection services)
    {
        services.AddRequestBus();
        services.TryAddScoped<IResponseComposer,ResponseComposer>();

        return services;
    }
}
