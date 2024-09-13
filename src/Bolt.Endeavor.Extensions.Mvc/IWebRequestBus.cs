using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bolt.Endeavor.Extensions.Mvc;

public interface IWebRequestBus
{
    Task<IResult> Result<TRequest, TResponse>(TRequest request, CancellationToken ct);
    Task<IResult> Result<TRequest>(TRequest request, CancellationToken ct);
    Task<IActionResult> ActionResult<TRequest, TResponse>(TRequest request, CancellationToken ct);
    Task<IActionResult> ActionResult<TRequest>(TRequest request, CancellationToken ct);
}

internal sealed class WebRequestBus(IRequestBus bus, ITraceIdProvider traceIdProvider) : IWebRequestBus
{
    public async Task<IResult> Result<TRequest, TResponse>(TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send<TRequest, TResponse>(request, ct);
        return rsp.ToResult(traceIdProvider.Get());
    }

    public async Task<IResult> Result<TRequest>(TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send(request, ct);
        return rsp.ToResult(traceIdProvider.Get());
    }

    public async Task<IActionResult> ActionResult<TRequest, TResponse>(TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send<TRequest, TResponse>(request, ct);
        return rsp.ToActionResult(traceIdProvider.Get());
    }

    public async Task<IActionResult> ActionResult<TRequest>(TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send(request, ct);
        return rsp.ToActionResult(traceIdProvider.Get());
    }
}