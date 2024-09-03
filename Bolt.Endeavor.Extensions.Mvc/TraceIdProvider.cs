using System.Diagnostics;
using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class TraceIdProvider(IHttpContextAccessor contextAccessor) : ITraceIdProvider
{
    public string Get()
    {
        var traceId = contextAccessor.HttpContext == null
            ? null
            : contextAccessor.HttpContext.Request.Headers.TryGetValue(Constants.HeaderTraceId, out var headerTraceId)
                ? headerTraceId.ToString()
                : null;

        if (!string.IsNullOrWhiteSpace(traceId)) return traceId;
        
        return Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
    }
}