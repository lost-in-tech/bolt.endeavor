namespace Bolt.Endeavor.Extensions.Mvc;

internal interface IDataKeySettings
{
    public string TraceIdHeaderName { get; }
    public string ConsumerIdHeaderName { get; }
    public string TenantHeaderName { get; }
    public string TenantRouteName { get; }
    public string TenantQueryName { get; }
    
    public string TraceIdLogKey { get; }
    public string TenantLogKey { get; }
    public string UserIdLogKey { get; }
    public string AppIdLogKey { get; }
    public string ConsumerIdLogKey { get; }
}