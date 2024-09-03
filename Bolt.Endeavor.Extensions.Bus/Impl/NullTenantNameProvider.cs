namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal sealed class NullTenantNameProvider : ICurrentTenantProvider
{
    public string Get()
    {
        return string.Empty;
    }
}