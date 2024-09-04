using Microsoft.Extensions.Hosting;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class DefaultLogScopeProvider(ITraceIdProvider traceIdProvider,
    ICurrentTenantProvider currentTenantProvider,
    ICurrentUserProvider currentUserProvider,
    IDataKeySettings options,
    IHttpContextWrapper httpContextWrapper,
    IHostEnvironment hostEnvironment) : ILogScopeProvider
{
    public IEnumerable<(string Name, object Value)> Get()
    {
        var traceId = traceIdProvider.Get();
        var tenant = currentTenantProvider.Get();
        var userId = currentUserProvider.Get().UserId;

        if (traceId.HasValue()) yield return (options.TraceIdLogKey, traceId);
        if (tenant.HasValue()) yield return (options.TenantLogKey, tenant);
        if (userId.HasValue()) yield return (options.UserIdLogKey, userId);

        var consumerId = httpContextWrapper.HeaderValue(options.ConsumerIdHeaderName);
        
        if (consumerId.HasValue()) yield return (options.ConsumerIdLogKey, consumerId);

        yield return (options.AppIdLogKey, hostEnvironment.ApplicationName);
    }
}