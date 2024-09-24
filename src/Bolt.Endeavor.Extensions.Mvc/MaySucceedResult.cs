using Bolt.Endeavor.Extensions.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class MaySucceedResult(MaySucceed value) : ActionResult, IResult
{
    public override Task ExecuteResultAsync(ActionContext context)
    {
        return ExecuteAsync(context.HttpContext);
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        if (value.IsSucceed)
        {
            httpContext.Response.StatusCode = value.StatusCode;

            var resourceUrl = value.StatusCode == 201 
                ? value.ResourceUrl()
                : value.RedirectUrl();

            AppendLocationHelper.AppendLocation(httpContext, resourceUrl);
            
            return;
        }
        
        httpContext.Response.StatusCode = value.Failure.StatusCode;

        var problemDetails = ProblemDetailsFactory.New(value.Failure,
            httpContext.RequestServices.GetRequiredService<ITraceIdProvider>().Get(),
            httpContext.Request.Path);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, CancellationToken.None);
    }
}

internal sealed class MaySucceedResult<T>(MaySucceed<T> value) : ActionResult, IResult
{
    public override Task ExecuteResultAsync(ActionContext context)
    {
        return ExecuteAsync(context.HttpContext);
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        if (value.IsSucceed)
        {
            var resourceUrl = value.StatusCode == 201 
                ? value.ResourceUrl()
                : value.RedirectUrl();
            
            AppendLocationHelper.AppendLocation(httpContext, resourceUrl);
            
            httpContext.Response.StatusCode = value.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(value.Value, CancellationToken.None);
            
            return;
        }
        
        httpContext.Response.StatusCode = value.Failure.StatusCode;

        var problemDetails = ProblemDetailsFactory.New(value.Failure,
            httpContext.RequestServices.GetRequiredService<ITraceIdProvider>().Get(),
            httpContext.Request.Path);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, CancellationToken.None);
    }

    
}

internal static class AppendLocationHelper
{
    public static void AppendLocation(HttpContext httpContext, string? location)
    {
        // Check if resourceUrl is null
        if (!string.IsNullOrWhiteSpace(location))
        {
            // Check if the resourceUrl is relative and needs to be converted to an absolute URL
            if (!Uri.IsWellFormedUriString(location, UriKind.Absolute))
            {
                // Get the request's base URL
                var request = httpContext.Request;
                var baseUri = $"{request.Scheme}://{request.Host}{request.PathBase}";

                // Combine the baseUri with the relative resourceUrl
                var absoluteUri = new Uri(new Uri(baseUri), location);
            
                // Set the Location header
                httpContext.Response.Headers.Location = absoluteUri.ToString();
            }
            else
            {
                // If the URL is already absolute, just set it
                httpContext.Response.Headers.Location = location;
            }
        }
    }
}