namespace Bolt.Endeavor.Extensions.Bus;

public static class RequestBusExtensions
{
    /// <summary>
    /// Send request for handler to process
    /// </summary>
    /// <param name="source"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <returns></returns>
    public static async Task<MaySucceed> Send<TRequest>(this IRequestBus source,  
        TRequest request, 
        CancellationToken ct = default)
    {
        var rsp = await source.Send<TRequest, None>(request, ct);

        if (rsp.IsSucceed) return MaySucceed.Ok();
        
        return rsp.Failure;
    }
}
