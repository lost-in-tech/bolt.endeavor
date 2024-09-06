using Bolt.Endeavor.Extensions.Bus;

namespace Bolt.Endeavor.Extensions.Composers;

public abstract class ResponseCollectionFilter<TRequest, TResponse> 
    : IProcessFilter<TRequest, ICollection<TResponse>>
{
    public int Priority => SystemPriorities.Default;

    public virtual Task<MaySucceed<TRequest>> Apply(IBusContext context, 
        TRequest request, 
        CancellationToken cancellationToken) 
        => Task.FromResult<MaySucceed<TRequest>>(request);


    Task<MaySucceed<ICollection<TResponse>>> IProcessFilter<TRequest, ICollection<TResponse>>.Apply(IBusContext context, 
        TRequest request, 
        MaySucceed<ICollection<TResponse>> response,
        CancellationToken cancellationToken)
    {
        return response.IsFailed 
                ? response.Failure.ToMaySucceedTask<ICollection<TResponse>>() 
                : Apply(context, request, response.Value ?? new List<TResponse>(), cancellationToken);
    }

    protected virtual Task<MaySucceed<ICollection<TResponse>>> Apply(IBusContext context, 
        TRequest request, 
        ICollection<TResponse> response,
        CancellationToken cancellationToken) => Task.FromResult(new MaySucceed<ICollection<TResponse>>(response));

    public bool IsApplicable(IBusContext context, TRequest request) => true;
}