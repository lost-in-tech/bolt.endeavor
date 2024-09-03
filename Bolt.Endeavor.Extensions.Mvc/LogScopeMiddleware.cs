using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Mvc;

public class LogScopeMiddleware(RequestDelegate next,
    IHostEnvironment hostEnvironment,
    IHttpContextAccessor contextAccessor,
    ILogger<LogScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var traceIdProvider = context.RequestServices.GetRequiredService<ITraceIdProvider>();
        var tenantProvider = context.RequestServices.GetRequiredService<ICurrentTenantProvider>();
        var userProvider = context.RequestServices.GetRequiredService<ICurrentUserProvider>();

        var traceId = traceIdProvider.Get();
        var tenant = tenantProvider.Get();
        var userId = userProvider.Get().UserId;

        var data = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(traceId)) data["traceId"] = traceId;
        if (!string.IsNullOrWhiteSpace(tenant)) data["tenant"] = tenant;
        if (!string.IsNullOrWhiteSpace(userId)) data["userId"] = userId;
        
        
        if (contextAccessor.HttpContext != null)
        {
            if (contextAccessor.HttpContext.Request.Headers.TryGetValue(Constants.HeaderConsumerId, out var consumerId))
            {
                data["consumerId"] = consumerId.ToString();
            }
        }
        
        data["appId"] = hostEnvironment.ApplicationName;
        
        using (logger.BeginScope(data))
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[Constants.HeaderTraceId] = traceId;

                return Task.CompletedTask;
            });
                
            await next(context);
        }
    }
}