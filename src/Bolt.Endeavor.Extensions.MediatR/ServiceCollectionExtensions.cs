using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bolt.Endeavor.Extensions.MediatR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidatorPipeline<TRequest>(this IServiceCollection services,
        Type type)
        where TRequest : IRequest<MaySucceed>
    {
        services.AddTransient(typeof(IPipelineBehavior<TRequest, MaySucceed>), type);

        return services;
    }
    
    public static IServiceCollection AddValidatorPipeline<TRequest,TResponse>(this IServiceCollection services,
        Type type)
        where TRequest : IRequest<MaySucceed<TResponse>>
    {
        services.AddTransient(typeof(IPipelineBehavior<TRequest, MaySucceed<TResponse>>), type);

        return services;
    }
}