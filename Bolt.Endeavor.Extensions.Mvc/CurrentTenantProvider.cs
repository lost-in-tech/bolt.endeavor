using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class CurrentTenantProvider(IHttpContextAccessor context) : ICurrentTenantProvider
{
    public string Get()
    {
        if (context.HttpContext == null) return string.Empty;

        var tenant = context.HttpContext.GetRouteValue("tenant")?.ToString();

        if (!string.IsNullOrWhiteSpace(tenant)) return tenant;
        
        if(context.HttpContext.Request.Query.TryGetValue("tenant", out var qTenant))
        {
            return qTenant.ToString();
        }

        return string.Empty;
    }
}