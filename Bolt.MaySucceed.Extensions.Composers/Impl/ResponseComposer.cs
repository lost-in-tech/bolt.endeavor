using Bolt.MaySucceed.Extensions.Bus;
using Bolt.MaySucceed.Extensions.Bus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bolt.MaySucceed.Extensions.Composers.Impl;

internal sealed class ResponseComposer : IResponseComposer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBusContextFactory _busContextFactory;
    private readonly ILogger<ResponseComposer> _logger;

    public ResponseComposer(IServiceProvider serviceProvider, IBusContextFactory busContextFactory, ILogger<ResponseComposer> logger)
    {
        _serviceProvider = serviceProvider;
        _busContextFactory = busContextFactory;
        _logger = logger;
    }

    public async Task<MaySucceed<ICollection<TResponse>>> Compose<TRequest, TResponse>(TRequest request, 
        CancellationToken cancellationToken)
    {
        var context = _busContextFactory.Create();

        var filters = _serviceProvider.GetServices<IProcessFilter<TRequest, ICollection<TResponse>>>().ToArray();

        var requestFilterRsp = await filters.ApplyRequestFilters(context, request, cancellationToken);

        if (requestFilterRsp.IsFailed) return requestFilterRsp.Failure;

        request = requestFilterRsp.Value ?? request;

        var providers = _serviceProvider.GetServices<IResponseProvider<TRequest, TResponse>>().ToArray();

        var result = new List<TResponse>();

        var rsp = await AppendResponse(context, request, false, providers, result, cancellationToken);

        if (rsp.IsFailed) return rsp.Failure;

        _ = await AppendResponse(context, request, true, providers, result, cancellationToken);

        return await filters.ApplyResponseFilters(context, request, result, cancellationToken);
    }

    private async Task<MaySucceed> AppendResponse<TRequest, TResponse>(IBusContextReader context, TRequest request, bool dependent,
        IEnumerable<IResponseProvider<TRequest, TResponse>> providers,
        ICollection<TResponse> responses,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task<MaySucceed<TResponse>>>();

        foreach (var provider in providers)
        {
            if (provider.ExecutionHint == ExecutionHint.Dependent)
            {
                if (!dependent) continue;
            }

            if (!provider.IsApplicable(context, request)) continue;

            tasks.Add(ExecuteProvider(provider, context, request, cancellationToken));
        }

        await Task.WhenAll(tasks);

        foreach (var task in tasks)
        {
            if (task.Result.IsFailed) return task.Result.Failure;

            if (task.Result.Value is null) continue;

            responses.Add(task.Result.Value);
        }

        return MaySucceed.Ok();
    }

    private async Task<MaySucceed<TResponse>> ExecuteProvider<TRequest, TResponse>(IResponseProvider<TRequest, TResponse> provider, 
        IBusContextReader context, 
        TRequest request,
        CancellationToken cancellationToken)
    {
        if (provider.ExecutionHint == ExecutionHint.Main) return await provider.Get(context, request, cancellationToken);

        try
        {
            var rsp = await provider.Get(context, request, cancellationToken);

            return rsp.Value!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{provider.GetType()} failed with message {e.Message}");

            return new MaySucceed<TResponse>(null!);
        }
    }
}