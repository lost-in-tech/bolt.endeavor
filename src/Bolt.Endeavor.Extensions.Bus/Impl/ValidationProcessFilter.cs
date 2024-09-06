using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal sealed class ValidationProcessFilter<TRequest, TResponse>(
    IEnumerable<IRequestValidator<TRequest>> validators,
    ILogger<RequestBus> logger)
    : ProcessFilter<TRequest, TResponse>
{
    public override async Task<MaySucceed<TRequest>> Apply(IBusContext context, 
        TRequest request, 
        CancellationToken cancellationToken)
    {
        foreach (var validator in validators.OrderBy(x => x.Priority))
        {
            if (validator.IsApplicable(context, request) == false) continue;

            var rsp = await validator.Validate(context, request, cancellationToken);

            if (!rsp.IsSucceed)
            {
                logger.LogDebug("Validation failed for {requestType}", typeof(TRequest));
                
                return rsp.Failure;
            }
        }

        return request;
    }

    public override int Priority => SystemPriorities.ValidationFilter;
}
