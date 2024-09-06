using Bolt.Endeavor.Extensions.Bus;

namespace Bolt.Endeavor.Extensions.Mvc;

public interface ICurrentTenantProvider
{
    string Get();
}

public static class CurrentTenantExtensions
{
    private const string TenantKey = "__current_tenant";
    
    
    /// <summary>
    /// Set current tenant name in bus context
    /// </summary>
    /// <param name="context"></param>
    /// <param name="tenant"></param>
    public static void Tenant(this IBusContext context, string tenant)
    {
        context.Set(TenantKey, tenant);
    }

    /// <summary>
    /// Allow to read current tenant value in bus context if available
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? Tenant(this IBusContextReader context)
    {
        return context.TryGet<string>(TenantKey);
    }
}