using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

internal static class TestServerHttp
{
    private static async Task<HttpApiResponse<TContent>> HttpSend<TInput,TContent>(HttpClient client, HttpMethod method, string url, TInput input,
        Dictionary<string, string> headers)
    {
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
            TContent? cnt;
            try
            {
                cnt = await rsp.Content.ReadFromJsonAsync<TContent>(JsonSerializerOptionsFactory.Create());
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to deserialize content {typeof(TContent).FullName} with {e.Message}", e);
            }

            return new HttpApiResponse<TContent>
            {
                StatusCode = rsp.StatusCode,
                Headers = Map(rsp.Headers),
                Content = cnt,
            };
        }

        ApiProblemDetails? problemDetails;

        try
        {
            problemDetails = await rsp.Content.ReadFromJsonAsync<ApiProblemDetails>();
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to deserialize response as ProblemDetails with {e.Message}", e);
        }
        
        return new HttpApiResponse<TContent>
        {
            Content = default,
            StatusCode = rsp.StatusCode,
            Headers = Map(rsp.Headers),
            ProblemDetails = problemDetails
        };
    }
    
    private static async Task<HttpApiResponse> HttpSend<TInput>(HttpClient client, HttpMethod method, string url, TInput input,
        Dictionary<string, string> headers)
    {
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
            return new HttpApiResponse
            {
                StatusCode = rsp.StatusCode,
                Headers = msg.Headers.ToDictionary(x => x.Key, v => v.Value.ToString() ?? string.Empty)
            };
        }
        
        ApiProblemDetails? problemDetails;
        try
        {
            problemDetails = await rsp.Content.ReadFromJsonAsync<ApiProblemDetails>();
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to deserialize response as ProblemDetails with {e.Message}", e);
        }

        return new HttpApiResponse
        {
            StatusCode = rsp.StatusCode,
            Headers = Map(rsp.Headers),
            ProblemDetails = problemDetails
        };
    }

    public static Task<HttpApiResponse<TContent>> HttpGet<TContent>(HttpClient client, string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(client, HttpMethod.Get, url, new None(), headers ?? new());
    }
    
    public static Task<HttpApiResponse<TContent>> HttpDelete<TContent>(HttpClient client, string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(client, HttpMethod.Delete, url, new None(), headers ?? new());
    }
    
    public static Task<HttpApiResponse> HttpDelete(HttpClient client, string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend(client, HttpMethod.Delete, url, new None(), headers ?? new());
    }
    
    public static Task<HttpApiResponse<TContent>> HttpPost<TContent>(HttpClient client, string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(client, HttpMethod.Post, url, new None(), headers ?? new());
    }
    
    public static Task<HttpApiResponse<TContent>> HttpPut<TContent>(HttpClient client, string url, Dictionary<string, string>? headers = null)
    {
        return HttpSend<None, TContent>(client, HttpMethod.Put, url, new None(), headers ?? new());
    }
    
    
    public static Task<HttpApiResponse<TContent>> HttpPost<TInput,TContent>(HttpClient client, string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return HttpSend<TInput, TContent>(client, HttpMethod.Post, url, input, headers ?? new());
    }
    
    public static Task<HttpApiResponse<TContent>> HttpPut<TInput,TContent>(HttpClient client, string url, TInput input, Dictionary<string, string>? headers = null)
    {
        return HttpSend<TInput, TContent>(client, HttpMethod.Put, url, input, headers ?? new());
    }
    
     

    private static Dictionary<string, string> Map(HttpResponseHeaders rspHeader)
    {
        var result = new Dictionary<string, string>();

        foreach (var header in rspHeader)
        {
            result[header.Key] = string.Join(",", header.Value);
        }

        return result;
    }
}