namespace Bolt.Endeavor.Extensions.Tracing;

public interface IHttpTracingHeadersProvider
{
    IEnumerable<(string Name, string Value)> Get();
}