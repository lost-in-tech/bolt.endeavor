using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Bolt.Endeavor.Extensions.Mvc;
using Bookworm.Catalogue.Api.Features.Shared.Ports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace Bookworm.Catalogue.Api.Tests.Fakes;

public class WebFixture : Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures.WebFixture<Program>
{
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IBooksRepository>(_ => Substitute.For<IBooksRepository>()));
    }
}