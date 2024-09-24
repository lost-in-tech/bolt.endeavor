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

            var resourceUrl = value.RedirectUrl();

            if (!string.IsNullOrWhiteSpace(resourceUrl))
            {
                httpContext.Response.Headers.Location = resourceUrl;
            }
            
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
            httpContext.Response.StatusCode = value.StatusCode;

            var resourceUrl = value.RedirectUrl();

            if (!string.IsNullOrWhiteSpace(resourceUrl))
            {
                httpContext.Response.Headers.Location = resourceUrl;
            }
            
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