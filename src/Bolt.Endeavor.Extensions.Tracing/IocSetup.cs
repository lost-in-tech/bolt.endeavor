using Bolt.Endeavor.Extensions.Tracing.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Bolt.Endeavor.Extensions.Tracing;

public static class IocSetup
{
    public static IServiceCollection AddTracingFeatures(this IServiceCollection services, TracingIocSetupOptions? options = null)
    {
        options ??= new TracingIocSetupOptions();
        
        services.TryAddTransient<TracingHttpMessageHandler>();
        services.TryAddTransient<IHttpMessageHandlerBuilderFilter,HttpMessageBuilder>();
        
        services.TryAddSingleton<ITracingKeySettings>(options);
        services.TryAddEnumerable(ServiceDescriptor.Transient<ILogScopeProvider, DefaultLogScopeProvider>());
        services.TryAddEnumerable(ServiceDescriptor.Transient<IHttpTracingHeadersProvider, HttpTracingHeadersProvider>());
        
        return services;
    }
}

public record TracingIocSetupOptions : ITracingKeySettings
{
    public string TraceIdLogKey { get; set; } = Constants.TraceIdLogKey;
    public string TenantLogKey { get; set; } = Constants.TenantLogKey;
    public string UserIdLogKey { get; set; } = Constants.UserIdLogKey;
    public string AppIdLogKey { get; set; } = Constants.AppIdLogKey;
    public string ConsumerIdLogKey { get; set; } = Constants.ConsumerIdLogKey;
    public string TraceIdHeaderKey { get; set; } = Constants.HeaderTraceId;
    public string AppIdHeaderKey { get; set; } = Constants.HeaderAppId;
    public string TenantHeaderKey { get; set; } = Constants.HeaderTenant;
}