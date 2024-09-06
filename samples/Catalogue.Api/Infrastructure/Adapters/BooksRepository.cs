using Bolt.IocScanner.Attributes;
using Bookworm.Catalogue.Api.Features.Shared.Ports;

namespace Bookworm.Catalogue.Api.Infrastructure.Adapters;

[AutoBind]
public class BooksRepository : IBooksRepository
{
    private static readonly List<BookRecord> _store = new()
    {
        new BookRecord
        {
            Id = "bw-100",
            Title = "title-1",
            Price = 12
        },
        new BookRecord
        {
            Id = "bw-200",
            Title = "title-2",
            Price = 15
        },
        new BookRecord
        {
            Id = "bw-300",
            Title = "title-3",
            Price = 15
        }
    };
    
    public Task<BookRecord?> GetById(string id, CancellationToken ct = default)
    {
        return Task.FromResult(_store.FirstOrDefault(x => x.Id == id));
    }
}