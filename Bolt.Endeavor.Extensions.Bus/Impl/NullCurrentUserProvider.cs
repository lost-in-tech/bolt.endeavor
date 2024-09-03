namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal sealed class NullCurrentUserProvider : ICurrentUserProvider
{
    public CurrentUser Get()
    {
        return new CurrentUser();
    }
}