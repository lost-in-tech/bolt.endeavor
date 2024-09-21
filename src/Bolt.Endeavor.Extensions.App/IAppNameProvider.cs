using System.Reflection;

namespace Bolt.Endeavor.Extensions.App;

public interface IAppNameProvider
{
    string Get();
}

internal sealed class AppNameProvider(string name) : IAppNameProvider
{
    public string Get() => name;
}