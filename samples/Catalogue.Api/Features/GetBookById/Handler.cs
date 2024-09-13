using Bolt.Endeavor;
using Bolt.Endeavor.Extensions.Bus;
using Bolt.IocScanner.Attributes;
using Bookworm.Catalogue.Api.Contracts;
using Bookworm.Catalogue.Api.Features.Shared.Ports;

namespace Bookworm.Catalogue.Api.Features.GetBookById;

[AutoBind]
public class Handler(
    IBooksRepository booksRepository, 
    ILogger<Handler> logger) 
    : RequestHandlerAsync<GetBookByIdRequest, GetBookByIdResponse>
{
    public override async Task<MaySucceed<GetBookByIdResponse>> Handle(
        IBusContextReader context, 
        GetBookByIdRequest request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Start retrieving book by id {id}", request.BookId);

        var book = await booksRepository.GetById(request.BookId, cancellationToken);

        if (book == null)
        {
            logger.LogWarning("Book not found with {id}", request.BookId);

            return HttpFailure.NotFound($"Book not found with id:{request.BookId}");
        }

        return new GetBookByIdResponse
        {
            Id = book.Id,
            Price = book.Price,
            Title = book.Title
        };
    }
}