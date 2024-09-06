namespace Bolt.Endeavor.Extensions.Tracing;

public interface ILogScopeProvider
{
    IEnumerable<(string Name, object Value)> Get();
}