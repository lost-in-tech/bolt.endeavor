using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Mvc;

internal class GlobalErrorHandler(ILogger<GlobalErrorHandler> logger, 
    IHostEnvironment environment,
    IDataKeySettings options) : IExceptionHandler
{
    private static readonly string[] EnvWhiteList = ["development","dev","local","test"];
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled error occured with {msg} {exceptionType}", exception.Message, exception.InnerException?.GetType().FullName ?? exception.GetType().FullName);

        var isDevEnv = EnvWhiteList.Any(env =>
            string.Equals(env, environment.EnvironmentName, StringComparison.OrdinalIgnoreCase));

        var statusCode = 500;
        var traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;
        
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.Headers[options.TraceIdHeaderName] = traceId; 
        await httpContext.Response.WriteAsJsonAsync(new 
        {
            TraceId = traceId,
            Title = isDevEnv ? $"{exception.GetType().FullName} : {exception.Message}" : "An unhandled error occured",
            Details = "Check log for details",
            Status = statusCode,
        }, cancellationToken);

        return true;
    }
}