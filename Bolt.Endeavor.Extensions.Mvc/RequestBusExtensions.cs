using System.Diagnostics;
using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class RequestBusExtensions
{
    public static async Task<IResult> Result<TRequest, TResponse>(this IRequestBus bus, TRequest request)
    {
        var rsp = await bus.Send<TRequest, TResponse>(request);

        
        if (rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect
            || rsp.StatusCode == HttpResult.HttpStatusCodeTempRedirect)
        {
            var redirectUrl = rsp.ResourceUrl();

            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return TypedResults.StatusCode(rsp.StatusCode);
            }

            return TypedResults.Redirect(redirectUrl, rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect);
        }
        
        
        if (rsp.IsFailed)
        {
            
            return BuildProblemDetailsResult(rsp.Failure);
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return TypedResults.Created(url, rsp.Value);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return TypedResults.Ok(rsp.Value);

        return TypedResults.StatusCode(rsp.StatusCode);
    }
    
    public static async Task<IResult> Result<TRequest>(this IRequestBus bus, TRequest request)
    {
        var rsp = await bus.Send(request);

        
        if (rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect
            || rsp.StatusCode == HttpResult.HttpStatusCodeTempRedirect)
        {
            var redirectUrl = rsp.ResourceUrl();

            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return TypedResults.StatusCode(rsp.StatusCode);
            }

            return TypedResults.Redirect(redirectUrl, rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect);
        }
        
        
        if (rsp.IsFailed)
        {
            return BuildProblemDetailsResult(rsp.Failure);
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return TypedResults.Created(url);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return TypedResults.Ok();

        return TypedResults.StatusCode(rsp.StatusCode);
    }

    private static ProblemHttpResult BuildProblemDetailsResult(Failure failure)
    {
        return TypedResults.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Extensions =
            {
                ["traceId"] = Activity.Current?.TraceId.ToString(),
                ["errors"] = failure.Errors?.Select(x => new 
                {
                    Name = x.PropertyName,
                    Reason = x.Message,
                    Code = x.Code
                })
            },
            Title = failure.Reason,
            Status = failure.StatusCode
        });
    }
}