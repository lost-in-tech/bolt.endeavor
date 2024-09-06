using Bolt.Endeavor.Extensions.Bus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal class RequestBus : IRequestBus
{
    private readonly IServiceProvider _sp;
    private readonly IBusContextFactory _busContextFactory;
    private readonly ILogger<RequestBus> _logger;

    public RequestBus(IServiceProvider sp, IBusContextFactory busContextFactory, ILogger<RequestBus> logger)
    {
        _sp = sp;
        _busContextFactory = busContextFactory;
        _logger = logger;
    }

    public async Task<MaySucceed> Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
    {
        var rsp = await Send<TRequest, None>(request, cancellationToken);

        return rsp.IsFailed ? rsp.Failure : MaySucceed.Ok(rsp.StatusCode);
    }

    public async Task<MaySucceed<TResponse>> Send<TRequest, TResponse>(
        TRequest request, 
        CancellationToken cancellationToken)
    {
        var context = _busContextFactory.Create();
        
        var filters = _sp.GetServices<IProcessFilter<TRequest, TResponse>>().ToArray();

        var requestFilterRsp = await filters.ApplyRequestFilters(context, request, cancellationToken);

        if (requestFilterRsp.IsFailed) return requestFilterRsp.Failure;

        request = requestFilterRsp.Value ?? request;

        var handlers = _sp.GetServices<IRequestHandler<TRequest, TResponse>>();

        foreach(var handler in handlers)
        {
            if (!handler.IsApplicable(context, request)) continue;

            var result = await handler.Handle(context, request, cancellationToken);

            return await filters.ApplyResponseFilters(context, request, result, cancellationToken);
        }

        throw new Exception("No handler found to handle request");        
    }

    public Task Publish<TEvent>(TEvent @event, CancellationToken ct)
    {
        var context = _busContextFactory.Create();
        
        var handlers = _sp.GetServices<IEventHandler<TEvent>>();

        var tasks = new List<Task>();
        
        foreach (var handler in handlers)
        {
            if(!handler.IsApplicable(context, @event)) continue;
            
            tasks.Add(handler.Handle(context, @event));
        }

        return Task.WhenAll(tasks);
    }
}
