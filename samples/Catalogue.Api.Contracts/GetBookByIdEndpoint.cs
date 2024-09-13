namespace Bookworm.Catalogue.Api.Contracts;

public static class GetBookByIdEndpoint
{
    public static string Path(string id) => $"api/v1/books/{id}";
    public static HttpMethod Method = HttpMethod.Get;
}

public class GetBookByIdRequest
{
    public string BookId { get; set; } = string.Empty;
}

public record GetBookByIdResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}