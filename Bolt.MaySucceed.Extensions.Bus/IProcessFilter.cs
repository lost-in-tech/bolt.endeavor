namespace Bolt.MaySucceed.Extensions.Bus;

public interface IProcessFilter<TRequest,TResponse>
{
    Task<MaySucceed<TRequest>> Apply(IBusContext context, 
        TRequest request, 
        CancellationToken cancellationToken = default);
    Task<MaySucceed<TResponse>> Apply(IBusContext context, 
        TRequest request, 
        MaySucceed<TResponse> response, 
        CancellationToken cancellationToken = default);
    bool IsApplicable(IBusContext context, TRequest request);
    int Priority { get; }
}

public abstract class ProcessFilter<TRequest, TResponse> : IProcessFilter<TRequest, TResponse>
{
    public virtual int Priority => SystemPriorities.Default;

    public virtual bool IsApplicable(IBusContext context, TRequest request) => true;

    public virtual Task<MaySucceed<TRequest>> Apply(IBusContext context, 
        TRequest request, 
        CancellationToken cancellationToken) 
        => Task.FromResult<MaySucceed<TRequest>>(request);

    public virtual Task<MaySucceed<TResponse>> Apply(IBusContext context, 
        TRequest request, 
        MaySucceed<TResponse> response,
        CancellationToken cancellationToken) 
        => Task.FromResult(response);
}

public abstract class ProcessFilter<TRequest> : IProcessFilter<TRequest, None>
{
    public virtual int Priority => SystemPriorities.Default;

    public virtual bool IsApplicable(IBusContext context, TRequest request) => true;

    public virtual Task<MaySucceed<TRequest>> Apply(IBusContext context, 
        TRequest request, 
        CancellationToken cancellationToken) 
        => Task.FromResult<MaySucceed<TRequest>>(request);

    public virtual Task<MaySucceed<None>> Apply(IBusContext context, TRequest request, 
        MaySucceed<None> response, 
        CancellationToken cancellationToken) 
        => Task.FromResult(response);
}