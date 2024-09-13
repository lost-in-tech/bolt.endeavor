using Bolt.Endeavor.Extensions.Bus;
using Bolt.Endeavor.Extensions.Mvc;
using Bookworm.Catalogue.Api.Contracts;
using Bookworm.Catalogue.Api.Features.Shared.Endpoints;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Catalogue.Api.Features.GetBookById;

public class Endpoint : EndpointBase<DefaultGroup>
{
    public override void Configure(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/books/{id}", Handle)
            .WithTags("Books")
            .Produces<GetBookByIdResponse>()
            .Produces<ApiProblemDetails>(400)
            .Produces<ApiProblemDetails>(404)
            .Produces<ApiProblemDetails>(500)
            .WithOpenApi();
    }

    private Task<IResult> Handle(IWebRequestBus bus, [FromRoute] string id, CancellationToken ct)
    {
        return bus.Result<GetBookByIdRequest, GetBookByIdResponse>(new GetBookByIdRequest
        {
            BookId = id
        }, ct);
    }
}