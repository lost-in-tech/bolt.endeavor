using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class LogScopeMiddleware(RequestDelegate next,
    IEnumerable<ILogScopeProvider> providers,
    IDataKeySettings settings,
    ITraceIdProvider traceIdProvider,
    ILogger<LogScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using (logger.BeginScope(BuildScopeData()))
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[settings.TraceIdHeaderName] = traceIdProvider.Get();

                return Task.CompletedTask;
            });
                
            await next(context);
        }
    }

    private Dictionary<string, string> BuildScopeData()
    {
        var result = new Dictionary<string, string>();
        
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