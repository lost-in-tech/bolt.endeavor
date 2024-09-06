namespace Bolt.Endeavor.Extensions.Tracing;

public interface ITracingKeySettings
{
    public string TraceIdLogKey { get; }
    public string TenantLogKey { get; }
    public string UserIdLogKey { get; }
    public string AppIdLogKey { get; }
    public string ConsumerIdLogKey { get; }
    
    public string TraceIdHeaderKey { get; }
    public string AppIdHeaderKey { get; }
    public string TenantHeaderKey { get; }
}