using Bolt.Endeavor;
using Catalogue.Api.Contracts;

namespace Bookworm.Orders.Api.Features.Shared.Ports;

public interface ICatalogueApiProxy
{
    Task<MaySucceed<GetBookByIdResponse>> GetById(GetBookByIdRequest request, CancellationToken ct);
}