using Bolt.MaySucceed.Extensions.Bus;

namespace Bolt.MaySucceed.Extensions.Composers;

public interface IResponseProvider<in TRequest,TResponse>
{
    Task<MaySucceed<TResponse>> Get(IBusContextReader context, TRequest request, CancellationToken cancellationToken);
    ExecutionHint ExecutionHint { get; }
    bool IsApplicable(IBusContextReader context, TRequest request);
}


public abstract class ResponseProvider<TRequest, TResponse> : IResponseProvider<TRequest, TResponse>
{
    public virtual ExecutionHint ExecutionHint => ExecutionHint.Dependent;

    public abstract Task<MaySucceed<TResponse>> Get(IBusContextReader context, TRequest request, CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}

public abstract class MainResponseProvider<TRequest, TResponse> : IResponseProvider<TRequest, TResponse>
{
    public ExecutionHint ExecutionHint => ExecutionHint.Main;

    public abstract Task<MaySucceed<TResponse>> Get(IBusContextReader context, TRequest request, CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}

public abstract class IndependentResponseProvider<TRequest, TResponse> : IResponseProvider<TRequest, TResponse>
{
    public ExecutionHint ExecutionHint => ExecutionHint.Independent;

    public abstract Task<MaySucceed<TResponse>> Get(IBusContextReader context, TRequest request, CancellationToken cancellationToken);

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;
}