using Bolt.Endeavor.Extensions.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class LogScopeMiddleware(RequestDelegate next,
    IEnumerable<ILogScopeProvider> providers,
    ITraceIdProvider traceIdProvider,
    ILogger<LogScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context, 
        ITracingKeySettings settings)
    {
        using (logger.BeginScope(BuildScopeData()))
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[settings.TraceIdHeaderKey] = traceIdProvider.Get();

                return Task.CompletedTask;
            });
                
            await next(context);
        }
    }

    private Dictionary<string, object> BuildScopeData()
    {
        var result = new Dictionary<string, object>();
        
        foreach (var provider in providers)
        {
            var data = provider.Get();

            foreach (var item in data)
            {
                result[item.Name] = item.Value;
            }
        }

        return result;
    }
}