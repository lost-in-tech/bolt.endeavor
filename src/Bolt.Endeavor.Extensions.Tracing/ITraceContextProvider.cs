namespace Bolt.Endeavor.Extensions.Tracing;

public interface ITraceContextProvider
{
    TraceContextDto Get();
}

public record TraceContextDto
{
    public string TraceId { get; set; } = string.Empty;
    public string? Tenant { get; set; }
    public string? UserId { get; set; }
    public string AppId { get; set; } = string.Empty;
    public string? ConsumerId { get; set; }
}