using MediatR;

namespace Bolt.Endeavor.Extensions.MediatR;

public record RequestBase : IRequest<MaySucceed>
{
}

public record RequestBase<TResponse> : IRequest<MaySucceed<TResponse>>
{
    
}