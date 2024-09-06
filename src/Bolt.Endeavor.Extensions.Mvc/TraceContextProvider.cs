using Bolt.Endeavor.Extensions.Tracing;
using Microsoft.AspNetCore.Hosting;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class TraceContextProvider(
    IHttpContextWrapper http,
    ITraceIdProvider traceIdProvider,
    ICurrentUserProvider currentUserProvider,
    ICurrentTenantProvider tenantProvider,
    IWebHostEnvironment hostEnvironment,
    ITracingKeySettings settings) : ITraceContextProvider
{
    public TraceContextDto Get()
    {
        return new TraceContextDto
        {
            TraceId = traceIdProvider.Get(),
            UserId = currentUserProvider.Get().UserId,
            Tenant = tenantProvider.Get(),
            AppId = hostEnvironment.ApplicationName,
            ConsumerId = http.HeaderValue(settings.AppIdHeaderKey)
        };
    }
}