namespace Bolt.Endeavor.Extensions.Bus;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<MaySucceed<TResponse>> Handle(IBusContextReader context, TRequest request, CancellationToken cancellationToken);
    bool IsApplicable(IBusContextReader context, TRequest request);
}

public abstract class RequestHandlerAsync<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    public abstract Task<MaySucceed<TResponse>> Handle(IBusContextReader context, 
        TRequest request, 
        CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}

public abstract class RequestHandlerAsync<TRequest> : IRequestHandler<TRequest, None>
{
    async Task<MaySucceed<None>> IRequestHandler<TRequest, None>.Handle(IBusContextReader context, 
        TRequest request, 
        CancellationToken cancellationToken)
    {
        var rsp = await Handle(context, request, cancellationToken);

        return rsp.IsSucceed ? new MaySucceed<None>(new None()) : rsp.Failure;
    }

    public abstract Task<MaySucceed> Handle(IBusContextReader context, TRequest request, CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}


public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    Task<MaySucceed<TResponse>> IRequestHandler<TRequest, TResponse>.Handle(IBusContextReader context, 
        TRequest request, 
        CancellationToken cancellationToken)
    {
        return Task.FromResult(Handle(context, request, cancellationToken));
    }

    public abstract MaySucceed<TResponse> Handle(IBusContextReader context, 
        TRequest request, 
        CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}

public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest, None>
{
    Task<MaySucceed<None>> IRequestHandler<TRequest, None>.Handle(IBusContextReader context, 
        TRequest request, 
        CancellationToken cancellationToken)
    {
        var rsp = Handle(context, request, cancellationToken);

        return Task.FromResult(rsp.IsSucceed ? new MaySucceed<None>(new None()) : rsp.Failure);
    }

    protected abstract MaySucceed Handle(IBusContextReader context, 
        TRequest request, 
        CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}