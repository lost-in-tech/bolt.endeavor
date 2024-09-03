namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal sealed class NullTraceIdProvider : ITraceIdProvider
{
    public string Get()
    {
        return string.Empty;
    }
}