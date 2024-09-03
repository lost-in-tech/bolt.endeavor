namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal sealed class DefaultContextPopulator(
    ICurrentTenantProvider tenantProvider,
    ITraceIdProvider traceIdProvider,
    ICurrentUserProvider currentUserProvider)
    : IBusContextPopulator
{
    public void Populate(IBusContext context)
    {
        context.TraceId(traceIdProvider.Get());
        context.Tenant(tenantProvider.Get());
        context.CurrentUser(currentUserProvider.Get());
    }
}