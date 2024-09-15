using Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;
using Bookworm.Catalogue.Api.Features.Shared.Ports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace Bookworm.Catalogue.Api.Tests.Fakes;

public sealed class WebFixture : WebFixtureBase<Program>
{
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IBooksRepository>(_ => Substitute.For<IBooksRepository>()));
    }
}

[CollectionDefinition(nameof(WebFixtureCollection), DisableParallelization = true)]
public class WebFixtureCollection : ICollectionFixture<WebFixture>
{
}
