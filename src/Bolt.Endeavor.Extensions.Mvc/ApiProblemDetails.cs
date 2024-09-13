namespace Bolt.Endeavor.Extensions.Mvc;

public class ApiProblemDetails
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public int Status { get; init; }
    public string? Details { get; init; }
    public string? Instance { get; init; }
    public string? TraceId { get; init; }
    public ApiProblemDetailError[]? Errors { get; init; }
}

public class ApiProblemDetailError
{
    public string? Code { get; init; }
    public required string Reason { get; init; }
    public string? Name { get; init; }
}