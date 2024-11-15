using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bolt.Endeavor.Extensions.App;
using Bolt.Endeavor.Extensions.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class IocSetup
{
    public static IServiceCollection AddRequestBusForMvc<T>(this IServiceCollection services,
        IConfiguration configuration,
        RequestBusMvcOptions? options = null)
    {
        options ??= new RequestBusMvcOptions();

        options = options with
        {
            AssembliesToScan = options.AssembliesToScan.Append(typeof(T).Assembly).ToArray()
        };
        
        return AddRequestBusForMvc(services, configuration, options);
    }

    public static IServiceCollection AddRequestBusForMvc(this IServiceCollection services,
        IConfiguration configuration,
        RequestBusMvcOptions? options = null)
    {
        options ??= new RequestBusMvcOptions();

        services.AddBoltEndeavorApp(configuration, new BoltEndeavorAppOptions
        {
            AppName = options.AppName
        });
        
        if (!options.SkipConfigureDefaultJsonOptions)
        {
            services.Configure<JsonOptions>(opt =>
            {
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.SerializerOptions.PropertyNameCaseInsensitive = true;
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                
                options.ConfigureJsonOptions?.Invoke(opt);
            });
        }

        services.AddSingleton<IDataKeySettings>(_ => options);

        services.TryAddSingleton<ITraceIdProvider, TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider, CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider, CurrentUserProvider>();

        services.AddLogging();
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IHttpContextWrapper, HttpContextWrapper>();
        services.TryAddSingleton<ITraceIdProvider, TraceIdProvider>();
        services.TryAddSingleton<ICurrentTenantProvider, CurrentTenantProvider>();
        services.TryAddSingleton<ICurrentUserProvider, CurrentUserProvider>();
        services.TryAddSingleton<IExceptionHandler, GlobalErrorHandler>();

        services.TryAddSingleton<ITraceContextProvider, TraceContextProvider>();
        services.AddTracingFeatures(options.TracingOptions);
        
        if (options.UseDefaultGlobalErrorHandler)
        {
            services.AddExceptionHandler<GlobalErrorHandler>();
        }

        services.AddHttpClient();
        services.AddEndpoints(options.AssembliesToScan);

        return services;
    }

    private static void AddServices(IServiceCollection services, Assembly assembly, IEnumerable<Type> openGenericTypes)
    {
        var types = assembly.GetTypes()
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
    public string? AppName { get; init; }
    public Assembly[] AssembliesToScan { get; init; } = [Assembly.GetExecutingAssembly()];
    public bool UseDefaultGlobalErrorHandler { get; init; } = true;

    public string TenantRouteName { get; init; } = "tenant";
    public string TenantQueryName { get; init; } = "tenant";
    
    public TracingIocSetupOptions? TracingOptions { get; init; }
    /// <summary>
    /// When set to false the following json serializer settings applied. Set the value to true
    /// If the want to configure json by yourself
    /// - Ignore null when writing null
    /// - Case insensitive
    /// - CamelCasePropertyName
    /// - String for enum
    /// </summary>
    public bool SkipConfigureDefaultJsonOptions { get; init; }
    
    /// <summary>
    /// If you want to apply default json option but want to amend on top of that.
    /// </summary>
    public Action<JsonOptions>? ConfigureJsonOptions { get; init; }
}