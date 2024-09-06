using System.Diagnostics.CodeAnalysis;

namespace Bolt.Endeavor.Extensions.Tracing.Impl;

internal sealed class DefaultLogScopeProvider(
    ITracingKeySettings options,
    ITraceContextProvider traceContextProvider) : ILogScopeProvider
{
    public IEnumerable<(string Name, object Value)> Get()
    {
        var traceContext = traceContextProvider.Get();
        
        if (HasValue(traceContext.TraceId)) yield return (options.TraceIdLogKey, traceContext.TraceId);
        if (HasValue(traceContext.Tenant)) yield return (options.TenantLogKey, traceContext.Tenant);
        if (HasValue(traceContext.UserId)) yield return (options.UserIdLogKey, traceContext.UserId);
        if (HasValue(traceContext.ConsumerId)) yield return (options.ConsumerIdLogKey, traceContext.ConsumerId);
        if (HasValue(traceContext.AppId)) yield return (options.AppIdLogKey, traceContext.AppId);
    }

    private bool HasValue([NotNullWhen(true)]string? value) => !string.IsNullOrWhiteSpace(value);
}