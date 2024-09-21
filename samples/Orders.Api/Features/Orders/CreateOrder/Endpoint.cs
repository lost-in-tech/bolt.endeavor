using Bolt.Endeavor.Extensions.App;
using Bolt.Endeavor.Extensions.Mvc;
using Bookworm.Orders.Api.Features.Shared.Endpoints;
using Orders.Api.Contracts;

namespace Bookworm.Orders.Api.Features.Orders.CreateOrder;

public class Endpoint : EndpointBase<DefaultGroup>
{
    public override void Configure(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/orders", Handle)
            .WithTags("Orders")
            .Produces<CreateOrderResponse>()
            .Produces<ApiProblemDetails>(400)
            .Produces<ApiProblemDetails>(500)
            .WithOpenApi();
    }

    private Task<IResult> Handle(IWebRequestBus bus, CreateOrderRequest request, CancellationToken ct)
        => bus.Result<CreateOrderRequest, CreateOrderResponse>(request, ct);
}