using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class RequestBusExtensions
{
    public static IActionResult ToActionResult<T>(this MaySucceed<T> rsp, 
        string? traceId = null,
        string? instance = null)
    {
        if (rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect
            || rsp.StatusCode == HttpResult.HttpStatusCodeTempRedirect)
        {
            var redirectUrl = rsp.ResourceUrl();

            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return new StatusCodeResult(rsp.StatusCode);
            }

            return new RedirectResult(redirectUrl, rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect);
        }
        
        
        if (rsp.IsFailed)
        {
            var problemDetails = ProblemDetailsFactory.New(
                rsp.Failure, 
                traceId ?? Activity.Current?.TraceId.ToString(), 
                instance);
            
            return new ObjectResult(problemDetails)
            {
                StatusCode = rsp.StatusCode
            };
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return new CreatedResult(url, rsp.Value);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return new ObjectResult(rsp.Value)
        {
            StatusCode = rsp.StatusCode
        };

        return new StatusCodeResult(rsp.StatusCode);
    }
    
    public static IActionResult ToActionResult(this MaySucceed rsp, 
        string? traceId = null, 
        string? instance = null)
    {
        if (rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect
            || rsp.StatusCode == HttpResult.HttpStatusCodeTempRedirect)
        {
            var redirectUrl = rsp.ResourceUrl();

            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return new StatusCodeResult(rsp.StatusCode);
            }

            return new RedirectResult(redirectUrl, rsp.StatusCode == HttpResult.HttpStatusCodePermRedirect);
        }
        
        
        if (rsp.IsFailed)
        {
            var problemDetails = ProblemDetailsFactory.New(
                rsp.Failure, 
                traceId ?? Activity.Current?.TraceId.ToString());
            
            return new ObjectResult(problemDetails)
            {
                StatusCode = rsp.StatusCode
            };
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return new CreatedResult(url, null);
        }

        return new StatusCodeResult(rsp.StatusCode);
    }

    public static IResult ToResult(this MaySucceed rsp, 
        string? traceId = null,
        string? instance = null)
    {
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
            return BuildProblemDetailsResult(ProblemDetailsFactory.New(rsp.Failure, traceId, instance));
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return TypedResults.Created(url);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return TypedResults.Ok();

        return TypedResults.StatusCode(rsp.StatusCode);
    }
    
    public static IResult ToResult<T>(this MaySucceed<T> rsp, 
        string? traceId = null,
        string? instance = null)
    {
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
            return BuildProblemDetailsResult(ProblemDetailsFactory.New(rsp.Failure, traceId, instance));
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return TypedResults.Created(url, rsp.Value);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return TypedResults.Ok(rsp.Value);

        return TypedResults.StatusCode(rsp.StatusCode);
    }

    private static IResult BuildProblemDetailsResult(ApiProblemDetails problem)
    {
        return TypedResults.Problem(new ProblemDetails
        {
            Status = problem.Status,
            Extensions = new Dictionary<string, object?>
            {
                ["details"] = problem.Details,
                ["traceId"] = problem.TraceId,
                ["instance"] = problem.Instance,
                ["errors"] = problem.Errors
            }
        });
    }
}