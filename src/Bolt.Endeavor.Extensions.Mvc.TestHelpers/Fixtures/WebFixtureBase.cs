using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

public abstract class WebFixtureBase<TEntry> :  
    WebApplicationFactory<TEntry> where TEntry : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var root = Directory.GetCurrentDirectory();
            var fileProvider = new PhysicalFileProvider(root);
            config.AddJsonFile(fileProvider, "TestSettings.json", true, false);
            
            ConfigureAppConfiguration(context, config);
        });
        
        builder.ConfigureTestServices(services =>
        {
            services.Replace<ITraceIdProvider, FakeTraceIdProvider>(asSingleton: true);
            ConfigureTestServices(services);
        });
        
        builder.UseEnvironment("test");
        builder.UseContentRoot(".");
        base.ConfigureWebHost(builder);
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
    }

    protected virtual void ConfigureAppConfiguration(
        WebHostBuilderContext context, 
        IConfigurationBuilder builder)
    {
    }

    public T GetRequiredService<T>() where T : notnull => this.Services.GetRequiredService<T>();
    public T? TryGetServiceOf<T, TImpl>() => Services.TryGetServiceOf<T,TImpl>();

    public T GetFakeService<T>() where T : class => Services.GetFakeService<T>();

    public T? TryGetFakeServiceOf<T, TImpl>() where T : class => Services.TryGetFakeServiceOf<T, TImpl>();

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
    
    public Task<HttpApiResponse> HttpPost<TInput>(string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpPost(CreateClient(), url, input, headers);
    }
    
    public Task<HttpApiResponse> HttpPut<TInput>(string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return TestServerHttp.HttpPut(CreateClient(), url, input, headers);
    }
}