using Bolt.Endeavor.Extensions.Mvc;

namespace Bookworm.Orders.Api.Features.Shared.Endpoints;

public class DefaultGroup : IGroupEndpoint
{
    public RouteGroupBuilder Get(IEndpointRouteBuilder builder)
    {
        return builder.MapGroup("api/v1");
    }
}