namespace Bolt.MaySucceed.Extensions.Bus;

public interface IRequestBus
{
    /// <summary>
    /// Send requests to process and return response which may succeed or not
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<MaySucceed<TResponse>> Send<TRequest, TResponse>(TRequest request, CancellationToken ct = default);
    
    /// <summary>
    /// Publish an event which will be handled by handler which implemented IEventHandler
    /// </summary>
    /// <param name="event"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task Publish<TEvent>(TEvent @event, CancellationToken ct);
}
