using Bolt.Endeavor.Extensions.Bus;

namespace Bolt.Endeavor.Extensions.Mvc;

public interface ITraceIdProvider
{
    string Get();
}

public static class TraceIdExtensions
{
    private const string TraceIdKey = "__trace_id__";
    
    /// <summary>
    /// Set trace id in bus context
    /// </summary>
    /// <param name="context"></param>
    /// <param name="traceId"></param>
    public static void TraceId(this IBusContext context, string traceId)
    {
        context.Set(TraceIdKey, traceId);
    }

    /// <summary>
    /// Get trace id from bus context if available
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? TraceId(this IBusContextReader context)
    {
        return context.TryGet<string>(TraceIdKey);
    }
}