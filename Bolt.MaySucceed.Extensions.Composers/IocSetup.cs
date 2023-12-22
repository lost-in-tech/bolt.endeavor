using Bolt.MaySucceed.Extensions.Bus;
using Bolt.MaySucceed.Extensions.Composers.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.MaySucceed.Extensions.Composers;

public static class IocSetup
{
    public static IServiceCollection AddResponseComposer(this IServiceCollection services)
    {
        services.AddRequestBus();
        services.TryAddScoped<IResponseComposer,ResponseComposer>();

        return services;
    }
}
