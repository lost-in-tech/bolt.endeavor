namespace Bolt.Endeavor.Extensions.Mvc;

public record ApiProblemDetails
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public string? Details { get; init; }
    public int Status { get; init; }
    public string? Instance { get; init; }
    public string? TraceId { get; init; }
    public string? Tenant { get; init; }
    public ApiProblemDetailsError[]? Errors { get; init; }
} 


public record ApiProblemDetailsError
{
    public string? Code { get; init; }
    public required string Reason { get; init; }
    public string? Name { get; init; }
}