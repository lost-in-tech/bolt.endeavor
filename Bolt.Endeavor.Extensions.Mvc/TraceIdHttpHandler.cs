using Bolt.Endeavor.Extensions.Bus;
using Microsoft.Extensions.Hosting;

namespace Bolt.Endeavor.Extensions.Mvc;

public sealed class TraceIdHttpHandler(ITraceIdProvider traceIdProvider,
    IHostEnvironment hostEnvironment) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var traceId = traceIdProvider.Get();

        if (!string.IsNullOrWhiteSpace(traceId))
        {
            request.Headers.Add(Constants.HeaderTraceId, traceId);
        }

        request.Headers.Add(Constants.HeaderConsumerId, Uri.EscapeDataString(hostEnvironment.ApplicationName));

        return base.SendAsync(request, cancellationToken);
    }
}