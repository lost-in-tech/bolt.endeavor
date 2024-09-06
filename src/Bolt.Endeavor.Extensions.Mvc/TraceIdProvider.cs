using System.Diagnostics;
using Bolt.Endeavor.Extensions.Tracing;
using Microsoft.AspNetCore.Http;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class TraceIdProvider(
    IHttpContextAccessor contextAccessor, 
    ITracingKeySettings options) 
    : ITraceIdProvider
{
    public string Get()
    {
        var traceId = contextAccessor.HttpContext == null
            ? null
            : contextAccessor.HttpContext.Request.Headers.TryGetValue(options.TraceIdHeaderKey, out var headerTraceId)
                ? headerTraceId.ToString()
                : null;

        if (!string.IsNullOrWhiteSpace(traceId)) return traceId;
        
        return Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
    }
}