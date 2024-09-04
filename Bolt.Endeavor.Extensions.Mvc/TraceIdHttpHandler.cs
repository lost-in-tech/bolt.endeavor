using Bolt.Endeavor.Extensions.Bus;
using Microsoft.Extensions.Hosting;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class TraceIdHttpHandler(ITraceIdProvider traceIdProvider,
    IHostEnvironment hostEnvironment,
    IDataKeySettings options) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var traceId = traceIdProvider.Get();

        if (!string.IsNullOrWhiteSpace(traceId))
        {
            request.Headers.Add(options.TraceIdHeaderName, traceId);
        }

        request.Headers.Add(options.ConsumerIdHeaderName, Uri.EscapeDataString(hostEnvironment.ApplicationName));

        return base.SendAsync(request, cancellationToken);
    }
}