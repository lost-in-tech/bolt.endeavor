using System.Net;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

public record HttpApiResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public Dictionary<string, string> Headers { get; init; } = new();
    public ApiProblemDetails? ProblemDetails { get; init; }
}


internal struct None{}

public record HttpApiResponse<T> : HttpApiResponse
{
    public T? Content { get; init; }
}