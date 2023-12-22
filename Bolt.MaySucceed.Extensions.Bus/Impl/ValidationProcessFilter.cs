namespace Bolt.MaySucceed.Extensions.Bus.Impl;

internal sealed class ValidationProcessFilter<TRequest,TResponse> : ProcessFilter<TRequest, TResponse>
{
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    public ValidationProcessFilter(IEnumerable<IRequestValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public override async Task<MaySucceed<TRequest>> Apply(IBusContext context, 
        TRequest request, 
        CancellationToken cancellationToken)
    {
        foreach (var validator in _validators.OrderBy(x => x.Priority))
        {
            if (validator.IsApplicable(context, request) == false) continue;

            var rsp = await validator.Validate(context, request, cancellationToken);

            if (!rsp.IsSucceed) return rsp.Failure;
        }

        return request;
    }

    public override int Priority => SystemPriorities.ValidationFilter;
}
