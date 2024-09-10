# Bolt.Endeavor

A tiny library that define a struct MaySucceed which helps 
to return succeed or failure result and provide implicit 
conversion of data that help to reduce number of lines of code you need to write 
and result better code. Lets see some examples:

Lets say you have a proxy class that can succeed ot fail. You can use `MaySucceed<T>` as response type
as below.

```csharp
public async Task<MaySucceed<GetBookByIdResponse>> GetById(
    GetBookByIdRequest request, 
    CancellationToken ct)
{
    var client = clientFactory.CreateClient(nameof(CatalogueApiProxy));

    var path = $"{settings.Value.BaseUrl}/{GetBookByIdEndpoint.Path(request.BookId)}";

    var rsp = await client.GetAsync(path, ct);

    if (rsp.IsSuccessStatusCode)
    { 
        var result = await rsp.Content.ReadFromJsonAsync<GetBookByIdResponse>(ct);

        return result!;
    }
    
    logger.LogError("Catalogue api for {path} failed with {statusCode}", path, rsp.StatusCode);

    return HttpFailure.InternalServerError("Catalogue api request failed");
}    
```

As you when the call to underlying API succeed, we 
can just return the T and that automatically convert 
to `MaySucceed<T>` where the value of `MaySucceed<T>.IsSucceed` property 
should be true and your can read the value using `MaySucceed<T>.Value`;

Similarly when the process fails you can just return a Failure instance
using `HttpFailure` helper. No need to create instance of `MaySucceed<T>` as Failure
will implicitly converted to `MaySucceed<T>` where `IsSucceed` value should be false
and problem details will be available in `Failure` property

You can propagate the failure easily in other classes. Here is an example:

```csharp
public async Task<MaySucceed<CreateOrderResponse>> Handle(CreateOrderRequest request, CancellationToken ct)
{
    var rsp = await proxy.GetById(new GetBookByIdRequest{ Id = request.BookId }, ct);
    
    if(rsp.IsFailed) return rsp.Failure;
    
    ....
    
    return new CreateOrderResponse
    {
        OrderId = orderId
    }
}
```

As you see we can just propagate the failure just by returning the failure
of the proxy method failure. At the same time when it succeed we can return the
`CreateOrderResponse` instance instead of manually creating an instance
of `MaySucceed<CreateOrderResponse>`

The library provide helpers for common failures e.g:

```csharp
public Task<MaySucceed> DoSomething()
{
    if(allGood) return MaySucceed.Ok();
    
    // or we can return not found
    return HttpFailure.NotFound("Book not found");
    
    // or internal server error
    return HttoFailure.InternalServerError("Catalogue api is down.");
    
    // or you can return an instance failure as badrequest
    return new Error("Email","Email is invalid");
    
    // or you can return an collection fo errors as bad request
    return HttpFailure.BadRequest([
            new Error("Email","Email is invalid"),
            new Error("Name","Name is required"),
        ]);
}

```

# Bolt.Endeavor.Extensions.Bus

A request bus based on MaySucceed response.

## How to use this library

Add following package in your project.

```xml
<PackageReference Include="Bolt.Endeavor.Extensions.Bus" Version="<latest version>"/>
```

Register request bus in your IOC.

```csharp
services.AddRequestBus();
```

Define a request dto.

```csharp
public record CreateBookRequest
{ ... }
```

a response dot.

```csharp
public record CreateBookResponse{...}
```

now define a handler to handle CreateBookRequest as below:

```csharp
public class CreateBookHandler : RequestHandlerAsync<CreateBookRequest, CreateBookResponse>
{
     public override async Task<MaySucceed<CreateBookResponse>> Handle(
        IBusContextReader context, 
        GetBookByIdRequest request, 
        CancellationToken cancellationToken) 
     {         
         //if(succeed) return new CreateBookResponse{...};
         
         //if(failed) return failure instance;
     }
}
```

Make sure you register this handler in your IOC as below:

```csharp
services.TryAddTransient<IRequestHandler<CreateBookRequest,CreateBookResponse>, CreateBookHandler>();
```

you can now call `IRequestbus.SendAsync<CreateBookRequest,CreateBookResponse>(new CreateBookRequest{...})`. This will execute the handler and any validator your defined for this Request.

