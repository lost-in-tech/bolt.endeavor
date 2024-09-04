namespace Bolt.Endeavor.Extensions.Mvc;

public interface ILogScopeProvider
{
    IEnumerable<(string Name, object Value)> Get();
}