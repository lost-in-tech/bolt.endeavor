using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

public class WebFixture<TEntry> : WebApplicationFactory<TEntry> where TEntry : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton<ITraceIdProvider,FakeTraceIdProvider>());
            ConfigureTestServices(services);
        });
        
        builder.UseEnvironment("test");
        builder.UseContentRoot(".");
        base.ConfigureWebHost(builder);
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
    }

    public T GetRequiredService<T>() where T : notnull => this.Services.GetRequiredService<T>();
    public T? TryGetServiceOf<T,TImpl>()
    {
        return Services.GetServices<T>().FirstOrDefault(x => x?.GetType() == typeof(TImpl));
    }
    
    public T GetFakeService<T>() where T : class
    {
        var result = this.Services.GetRequiredService<T>();
        result.ClearSubstitute();
        result.ClearReceivedCalls();

        return result;
    }

    public T? TryGetFakeServiceOf<T,TImpl>() where T : class
    {
        var result = Services.GetServices<T>().FirstOrDefault(x => x?.GetType() == typeof(TImpl));
        result.ClearSubstitute();
        result.ClearReceivedCalls();
        return result;
    }

    public Task<HttpApiResponse<TContent>> HttpGet<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpGet<TContent>(CreateClient(), url, headers);
    }
    
    public Task<HttpApiResponse<TContent>> HttpDelete<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpDelete<TContent>(CreateClient(), url, headers);
    }
    
    public Task<HttpApiResponse> HttpDelete(string url, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpDelete(CreateClient(), url, headers);
    }
    
    public Task<HttpApiResponse<TContent>> HttpPost<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpPost<TContent>(CreateClient(), url, headers);
    }
    
    public Task<HttpApiResponse<TContent>> HttpPut<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpPut<TContent>(CreateClient(), url, headers);
    }
    
    
    public Task<HttpApiResponse<TContent>> HttpPost<TInput,TContent>(string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpPost<TInput,TContent>(CreateClient(), url, input, headers);
    }
    
    public Task<HttpApiResponse<TContent>> HttpPut<TInput,TContent>(string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpPut<TInput,TContent>(CreateClient(), url, input, headers);
    }
}
