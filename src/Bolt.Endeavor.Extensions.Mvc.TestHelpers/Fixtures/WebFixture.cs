using System.Net;
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

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

public class WebFixture<TEntry> : WebApplicationFactory<TEntry> where TEntry : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices((services) =>
        {
            services.Replace(ServiceDescriptor.Singleton<ITraceIdProvider>(_ =>
            {
                var traceIdProvider = Substitute.For<ITraceIdProvider>();
                traceIdProvider.Get().Returns("fake-trace-id-value");
                return traceIdProvider;
            }));
            
            ConfigureTestServices(services);
        });
        builder.UseEnvironment("test");
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

    private async Task<ApiResponse<TContent>> HttpSend<TInput,TContent>(HttpMethod method, string url, TInput input,
        Dictionary<string, string> headers)
    {
        var client = CreateClient();
        using var msg = new HttpRequestMessage(method, url);
        foreach (var header in headers)
        {
            msg.Headers.Add(header.Key, header.Value);
        }

        if (input is not None)
        {
            msg.Content = new StringContent(
                JsonSerializer.Serialize(input, JsonSerializerOptionsFactory.Create()),
                Encoding.UTF8,
                "application/json");
        }
        
        using var rsp = await client.SendAsync(msg);
        
        if (rsp.IsSuccessStatusCode)
        {
            var cnt = await rsp.Content.ReadFromJsonAsync<TContent>(JsonSerializerOptionsFactory.Create());

            return new ApiResponse<TContent>
            {
                Content = cnt,
                StatusCode = rsp.StatusCode,
                Headers = msg.Headers.ToDictionary(x => x.Key, v => v.Value.ToString() ?? string.Empty)
            };
        }

        return new ApiResponse<TContent>()
        {
            Content = default,
            StatusCode = rsp.StatusCode,
            Headers = Map(rsp.Headers),
            ProblemDetails = await rsp.Content.ReadFromJsonAsync<ApiProblemDetails>()
        };
    }
    
    private async Task<ApiResponse> HttpSend<TInput>(HttpMethod method, string url, TInput input,
        Dictionary<string, string> headers)
    {
        var client = CreateClient();
        using var msg = new HttpRequestMessage(method, url);
        foreach (var header in headers)
        {
            msg.Headers.Add(header.Key, header.Value);
        }

        if (input is not None)
        {
            msg.Content = new StringContent(
                JsonSerializer.Serialize(input, JsonSerializerOptionsFactory.Create()),
                Encoding.UTF8,
                "application/json");
        }

        msg.Method = method;
        
        using var rsp = await client.SendAsync(msg);
        
        if (rsp.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                StatusCode = rsp.StatusCode,
                Headers = msg.Headers.ToDictionary(x => x.Key, v => v.Value.ToString() ?? string.Empty)
            };
        }

        return new ApiResponse
        {
            StatusCode = rsp.StatusCode,
            Headers = Map(rsp.Headers),
            ProblemDetails = await rsp.Content.ReadFromJsonAsync<ApiProblemDetails>()
        };
    }

    public Task<ApiResponse<TContent>> HttpGet<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(HttpMethod.Get, url, new None(), headers ?? new());
    }
    
    public Task<ApiResponse<TContent>> HttpDelete<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(HttpMethod.Delete, url, new None(), headers ?? new());
    }
    
    public Task<ApiResponse> HttpDelete(string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend(HttpMethod.Delete, url, new None(), headers ?? new());
    }
    
    public Task<ApiResponse<TContent>> HttpPost<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(HttpMethod.Post, url, new None(), headers ?? new());
    }
    
    public Task<ApiResponse<TContent>> HttpPut<TContent>(string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(HttpMethod.Put, url, new None(), headers ?? new());
    }
    
    
    public Task<ApiResponse<TContent>> HttpPost<TInput,TContent>(string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return HttpSend<TInput, TContent>(HttpMethod.Post, url, input, headers ?? new());
    }
    
    public Task<ApiResponse<TContent>> HttpPut<TInput,TContent>(string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return HttpSend<TInput, TContent>(HttpMethod.Put, url, input, headers ?? new());
    }
    
     

    private Dictionary<string, string> Map(HttpResponseHeaders rspHeader)
    {
        var result = new Dictionary<string, string>();

        foreach (var header in rspHeader)
        {
            result[header.Key] = string.Join(",", header.Value);
        }

        return result;
    }
}

internal struct None{}

public record ApiResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public Dictionary<string, string> Headers { get; init; } = new();
    public ApiProblemDetails? ProblemDetails { get; init; }
}

public record ApiResponse<T> : ApiResponse
{
    public T? Content { get; init; }
}