namespace Bolt.MaySucceed.Extensions.Bus.Extensions;
public static class ProcessFiltersExtensions
{
    public static async Task<MaySucceed<TRequest>> ApplyRequestFilters<TRequest, TResponse>(
        this IEnumerable<IProcessFilter<TRequest, TResponse>> source, 
        IBusContext context, 
        TRequest request,
        CancellationToken cancellationToken)
    {
        var filtersByHighPriority = source.OrderBy(x => x.Priority);

        foreach (var filter in filtersByHighPriority)
        {
            var filteredRequestRsp = await filter.Apply(context, request, cancellationToken);

            if (filteredRequestRsp.IsFailed) return filteredRequestRsp.Failure;

            request = filteredRequestRsp.Value ?? request;
        }

        return request;
    }

    public static async Task<MaySucceed<TResponse>> ApplyResponseFilters<TRequest, TResponse>(
        this IEnumerable<IProcessFilter<TRequest, TResponse>> source, 
        IBusContext context, 
        TRequest request, 
        MaySucceed<TResponse> response,
        CancellationToken cancellationToken)
    {
        var filtersByLowPriority = source.OrderByDescending(x => x.Priority);

        MaySucceed<TResponse> filterResult = response;

        foreach (var filter in filtersByLowPriority)
        {
            filterResult = await filter.Apply(context, request, filterResult, cancellationToken);

            if (filterResult.IsFailed) return filterResult.Failure;
        }

        return filterResult;
    }
}
