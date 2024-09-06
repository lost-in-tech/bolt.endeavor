using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Bolt.Endeavor.Extensions.Tracing.Impl;

internal sealed class HttpMessageBuilder(IServiceProvider sp) : IHttpMessageHandlerBuilderFilter
{
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            next(builder);

            var handler = sp.GetRequiredService<TracingHttpMessageHandler>();
            
            builder.AdditionalHandlers.Add(handler);
        };
    }
}

internal sealed class TracingHttpMessageHandler(IEnumerable<IHttpTracingHeadersProvider> providers) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        foreach (var provider in providers)
        {
            var headers = provider.Get();

            foreach (var header in headers)
            {
                request.Headers.Add(header.Name, header.Value);
            }
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}