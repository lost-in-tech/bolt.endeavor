using Bolt.Endeavor;
using Bolt.Endeavor.Extensions.Bus;
using Bookworm.Orders.Api.Features.Shared.Ports;
using Orders.Api.Contracts;

namespace Bookworm.Orders.Api.Features.Orders.CreateOrder;

internal sealed class Handler(
    ICatalogueApiProxy catalogueApiProxy) 
    : RequestHandlerAsync<CreateOrderRequest, CreateOrderResponse>
{
    public override async Task<MaySucceed<CreateOrderResponse>> Handle(
        IBusContextReader context, 
        CreateOrderRequest request, 
        CancellationToken cancellationToken)
    {
        var bookDetails = await catalogueApiProxy.GetById(new(){ BookId = request.BookId}, cancellationToken);

        if (bookDetails.IsFailed) return bookDetails.Failure;

        var item = new OrderItem
        {
            BookId = bookDetails.Value.Id,
            Price = bookDetails.Value.Price,
            Quantity = request.Quantity
        };

        var rsp = new CreateOrderResponse(id: "ord-1", item: item, total: item.Price * item.Quantity);

        return HttpResult.Created(rsp, $"/api/v1/orders/{rsp.Id}");
    }
}