using Bolt.Endeavor.Extensions.Bus;

namespace Bolt.Endeavor.Extensions.Mvc;

public class MetaDataFilter<TRequest,TResponse>(
    ICurrentTenantProvider tenantProvider,
    ITraceIdProvider traceIdProvider,
    ICurrentUserProvider currentUserProvider) 
    : ProcessFilter<TRequest, TResponse>
{
    public override Task<MaySucceed<TResponse>> Apply(
        IBusContext context, 
        TRequest request, 
        MaySucceed<TResponse> response, 
        CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.Get();
        var traceId = traceIdProvider.Get();
        var currentUser = currentUserProvider.Get();

        var metaData = response.MetaData ?? new Dictionary<string, object>();
        metaData.Tenant(tenant);
        metaData.TraceId(traceId);
        if (!string.IsNullOrWhiteSpace(currentUser.UserId))
        {
            metaData.CurrentUser(currentUser);
        }

        return Task.FromResult(response with
        {
            MetaData = metaData
        });
    }

    public override int Priority => 10000;
}