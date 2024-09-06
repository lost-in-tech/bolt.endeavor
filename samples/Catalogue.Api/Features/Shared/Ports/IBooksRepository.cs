namespace Bookworm.Catalogue.Api.Features.Shared.Ports;

public interface IBooksRepository
{
    Task<BookRecord?> GetById(string id, CancellationToken ct = default);
}

public record BookRecord
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public decimal Price { get; set; }
}