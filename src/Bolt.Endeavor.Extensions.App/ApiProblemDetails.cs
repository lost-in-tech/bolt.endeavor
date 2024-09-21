namespace Bolt.Endeavor.Extensions.App;

public record ApiProblemDetails
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public int Status { get; init; }
    public string? Details { get; init; }
    public string? Instance { get; init; }
    public string? TraceId { get; init; }
    public ApiProblemDetailError[]? Errors { get; init; }
}

public record ApiProblemDetailError
{
    public string? Code { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string? Name { get; init; }
}

public sealed class ProblemDetailsFactory
{
    public static ApiProblemDetails New(Failure failure, 
        string? traceId = null,
        string? instance = null)
    {
        Defaults.TryGetValue(failure.StatusCode, out (string Type, string Title) defaults);
        
        return new ApiProblemDetails
        {
            Type = defaults.Type,
            Title = defaults.Title,
            Status = failure.StatusCode,
            Details = failure.Reason,
            TraceId = traceId,
            Instance = instance,
            Errors = failure.Errors?.Select(x => new ApiProblemDetailError
            {
                Reason = x.Message,
                Code = x.Code,
                Name = x.PropertyName
            }).ToArray()
        };
    }
    
    private static readonly Dictionary<int, (string Type, string Title)> Defaults = new()
    {
        [400] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            "Bad Request"
        ),

        [401] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            "Unauthorized"
        ),

        [403] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            "Forbidden"
        ),

        [404] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            "Not Found"
        ),

        [405] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.6",
            "Method Not Allowed"
        ),

        [406] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.7",
            "Not Acceptable"
        ),

        [408] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.9",
            "Request Timeout"
        ),

        [409] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            "Conflict"
        ),

        [412] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.13",
            "Precondition Failed"
        ),

        [415] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.16",
            "Unsupported Media Type"
        ),

        [422] =
        (
            "https://tools.ietf.org/html/rfc4918#section-11.2",
            "Unprocessable Entity"
        ),

        [426] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.5.22",
            "Upgrade Required"
        ),

        [500] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            "An error occurred while processing your request."
        ),

        [502] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.6.3",
            "Bad Gateway"
        ),

        [503] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.6.4",
            "Service Unavailable"
        ),

        [504] =
        (
            "https://tools.ietf.org/html/rfc9110#section-15.6.5",
            "Gateway Timeout"
        ),
    };
}