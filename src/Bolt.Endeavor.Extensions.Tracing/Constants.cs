namespace Bolt.Endeavor.Extensions.Tracing;

internal static class Constants
{
    public const string HeaderTraceId = "x-trace-id";
    public const string HeaderAppId = "x-app-id";
    public const string HeaderTenant = "x-tenant";

    public const string TraceIdLogKey = "traceId";
    public const string UserIdLogKey = "userId";
    public const string TenantLogKey = "tenant";
    public const string ConsumerIdLogKey = "consumerId";
    public const string AppIdLogKey = "appId";
}