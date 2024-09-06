namespace Bolt.Endeavor.Extensions.Bus;
public interface IRequestValidator<TRequest>
{
    ValueTask<MaySucceed> Validate(IBusContextReader context, TRequest input, CancellationToken cancellationToken = default);


    bool IsApplicable(IBusContextReader context, TRequest request);

    /// <summary>
    /// Priority in ascending order. 0 is high priority than 1. Default value is 100
    /// </summary>
    int Priority { get; }
}

public abstract class RequestValidatorAsync<TRequest> : IRequestValidator<TRequest>
{
    public virtual int Priority => SystemPriorities.Default;

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;

    public abstract ValueTask<MaySucceed> Validate(IBusContextReader context, 
        TRequest input, 
        CancellationToken cancellationToken);
}

public abstract class RequestValidator<TRequest> : IRequestValidator<TRequest>
{
    public virtual int Priority => SystemPriorities.Default;

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;

    ValueTask<MaySucceed> IRequestValidator<TRequest>.Validate(IBusContextReader context, TRequest input,
        CancellationToken cancellationToken) =>
        new(Validate(input));

    protected abstract MaySucceed Validate(TRequest input);
}
