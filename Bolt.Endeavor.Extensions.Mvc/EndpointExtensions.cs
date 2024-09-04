using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services)
    {
        return services.AddEndpoints([Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()]);
    }
    
    public static IServiceCollection AddEndpoints<T>(
        this IServiceCollection services)
    {
        return services.AddEndpoints([typeof(T).Assembly]);
    }
    
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddServices(services, assembly, typeof(IEndpoint));
            AddServices(services, assembly, typeof(IGroupEndpoint));
        }

        return services;
    }

    private static void AddServices(IServiceCollection services, Assembly assembly, Type interfaceType)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(interfaceType))
            .Select(type => ServiceDescriptor.Transient(interfaceType, type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
    }
    
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app)
    {
        IEnumerable<IGroupEndpoint> groupEndpoints = app.Services.GetServices<IGroupEndpoint>();
        
        IEnumerable<IEndpoint> endpoints = app.Services.GetServices<IEndpoint>().ToArray();

        foreach (var simpleEndpoint in endpoints)
        {
            if (string.IsNullOrWhiteSpace(simpleEndpoint.GroupName))
            {
                simpleEndpoint.Configure(app);
            }
        }

        foreach (var groupEndpointProvider in groupEndpoints)
        {
            var groupName = groupEndpointProvider.GetType().AssemblyQualifiedName;
            var groupEndpoint = groupEndpointProvider.Get(app);
            
            var endpointsForGroup = endpoints.Where(x => x.GroupName == groupName).ToArray();

            foreach (var endpointForGroup in endpointsForGroup)
            {
                endpointForGroup.Configure(groupEndpoint);
            }
        }

        return app;
    }
}