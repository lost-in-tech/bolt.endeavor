using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Replace<TService>(
        this IServiceCollection services,
        bool asSingleton,
        Func<IServiceProvider, TService> getImpl) where TService: class
    {
        services.Replace(asSingleton
            ? ServiceDescriptor.Singleton<TService>(getImpl.Invoke)
            : ServiceDescriptor.Scoped<TService>(getImpl.Invoke));

        return services;
    }

    public static IServiceCollection Replace<TService, TImpl>(
        this IServiceCollection services,
        bool asSingleton) 
        where TService : class
        where TImpl : class, TService
    {
        services.Replace(asSingleton
            ? ServiceDescriptor.Singleton<TService,TImpl>()
            : ServiceDescriptor.Scoped<TService,TImpl>());

        return services;
    }
}