using Bolt.Endeavor;
using Bolt.Endeavor.Extensions.Mvc;
using Bolt.IocScanner.Attributes;
using Bookworm.Orders.Api.Features.Shared.Ports;
using Catalogue.Api.Contracts;
using Microsoft.Extensions.Options;

namespace Bookworm.Orders.Api.Infrastructure.Ports;

[AutoBind]
internal sealed class CatalogueApiProxy(
    IHttpClientFactory clientFactory,
    IOptions<CatalogueApiSettings> settings,
    ILogger<CatalogueApiProxy> logger) 
    : ICatalogueApiProxy
{
    public async Task<MaySucceed<GetBookByIdResponse>> GetById(
        GetBookByIdRequest request, 
        CancellationToken ct)
    {
        var client = clientFactory.CreateClient(nameof(CatalogueApiProxy));

        var path = $"{settings.Value.BaseUrl}/{GetBookByIdEndpoint.Path(request.BookId)}";

        var rsp = await client.GetAsync(path, ct);

        if (rsp.IsSuccessStatusCode)
        { 
            var result = await rsp.Content.ReadFromJsonAsync<GetBookByIdResponse>(ct);

            return result!;
        }
        
        logger.LogError("Catalogue ap get books by id failed {path} {statusCode}", path, rsp.StatusCode);

        return HttpFailure.InternalServerError("Catalogue api request failed");
    }
}

[BindFromConfig]
public class CatalogueApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
}