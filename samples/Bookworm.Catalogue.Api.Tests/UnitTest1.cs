using System.Net.Http.Json;
using Catalogue.Api.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Bookworm.Catalogue.Api.Tests;

public class UnitTest1(WebApplicationFactory<Program> fixture) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Test1()
    {
        var rsp = await fixture.CreateClient().GetFromJsonAsync<GetBookByIdResponse>("/api/v1/books/bw-100");

        rsp.ShouldNotBeNull();
        rsp.Id.ShouldBe("bw-100");
    }
}