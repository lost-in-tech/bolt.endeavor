using MediatR;

namespace Bolt.Endeavor.Extensions.MediatR;

public abstract class RequestHandlerBase<TRequest> 
    : IRequestHandler<TRequest, MaySucceed> 
    where TRequest : IRequest<MaySucceed>
{
    public abstract Task<MaySucceed> Handle(TRequest request, CancellationToken ct);
}

public abstract class RequestHandlerBase<TRequest,TResponse> 
    : IRequestHandler<TRequest, MaySucceed<TResponse>> 
    where TRequest : IRequest<MaySucceed<TResponse>>
{
    public abstract Task<MaySucceed<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}