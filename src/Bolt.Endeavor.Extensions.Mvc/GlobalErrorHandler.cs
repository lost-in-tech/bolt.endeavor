using System.Diagnostics;
using Bolt.Endeavor.Extensions.App;
using Bolt.Endeavor.Extensions.Tracing;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Mvc;

internal class GlobalErrorHandler(
    ILogger<GlobalErrorHandler> logger,
    IHostEnvironment environment,
    ITracingKeySettings settings) : IExceptionHandler
{
    private static readonly string[] EnvWhiteList = ["development", "dev", "local", "test"];

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled error occured with {msg} {exceptionType}", exception.Message,
            exception.InnerException?.GetType().FullName ?? exception.GetType().FullName);

        var traceContextProvider = httpContext.RequestServices.GetRequiredService<ITraceIdProvider>();
        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
        
        var isDevEnv = EnvWhiteList.Any(env =>
            string.Equals(env, environment.EnvironmentName, StringComparison.OrdinalIgnoreCase));

        var statusCode = 500;
        var traceId = traceContextProvider.Get();

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.Headers[settings.TraceIdHeaderKey] = traceId;

        var problemDetails = ProblemDetailsFactory.New(
            new Failure(isDevEnv
                ? $"{exception.GetType().FullName} : {exception.Message}"
                : "An unhandled error occured")
            {
                StatusCode = statusCode
            }, traceId, exceptionHandlerFeature?.Path ?? httpContext.Request.Path);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}