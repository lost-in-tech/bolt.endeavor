using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bolt.Endeavor.Extensions.Mvc;

internal interface IHttpContextWrapper
{
    string? QueryValue(string name);
    string? RouteValue(string name);
    string? HeaderValue(string name);
    string? RequestPath();
}

internal sealed class HttpContextWrapper(IHttpContextAccessor contextAccessor) : IHttpContextWrapper
{
    public string? QueryValue(string name)
    {
        if (contextAccessor.HttpContext == null) return null;
        return contextAccessor.HttpContext.Request.Query.TryGetValue(name, out var value) ? value.ToString() : null;
    }

    public string? RouteValue(string name)
    {
        if (contextAccessor.HttpContext == null) return null;
        return contextAccessor.HttpContext.GetRouteValue(name)?.ToString();
    }

    public string? HeaderValue(string name)
    {
        if (contextAccessor.HttpContext == null) return null;
        return contextAccessor.HttpContext.Request.Headers.TryGetValue(name, out var value) ? value.ToString() : null;
    }

    public string? RequestPath()
    {
        return contextAccessor.HttpContext?.Request.Path;
    }
}