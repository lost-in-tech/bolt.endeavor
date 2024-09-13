namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

internal class FakeTraceIdProvider : ITraceIdProvider
{
    public string Get()
    {
        return "fake-trace-id-value";
    }
}