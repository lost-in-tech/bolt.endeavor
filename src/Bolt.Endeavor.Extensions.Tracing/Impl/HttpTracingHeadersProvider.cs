using System.Diagnostics.CodeAnalysis;

namespace Bolt.Endeavor.Extensions.Tracing.Impl;

internal sealed class HttpTracingHeadersProvider(
    ITracingKeySettings options,
    ITraceContextProvider traceContextProvider) 
    : IHttpTracingHeadersProvider
{
    public IEnumerable<(string Name, string Value)> Get()
    {
        var traceContext = traceContextProvider.Get();
        
        if (HasValue(traceContext.TraceId)) yield return (options.TraceIdHeaderKey, traceContext.TraceId);
        if (HasValue(traceContext.AppId)) yield return (options.AppIdHeaderKey, traceContext.AppId);
    }

    private bool HasValue([NotNullWhen(true)]string? value) => !string.IsNullOrWhiteSpace(value);
}