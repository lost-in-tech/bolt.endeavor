using System.Diagnostics;
using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class RequestBusExtensions
{
    public static IActionResult ToActionResult<T>(this MaySucceed<T> rsp, string? traceId = null)
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
            return new ObjectResult(new ApiProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Status = rsp.StatusCode,
                Title = rsp.Failure.Reason,
                TraceId = traceId ?? Activity.Current?.TraceId.ToString(),
                Errors = rsp.Failure.Errors?.Select(x => new ApiProblemDetailError()
                {
                    Code = x.Code,
                    Reason = x.Message,
                    Name = x.PropertyName
                }).ToArray()
            })
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
    
    public static IActionResult ToActionResult(this MaySucceed rsp, string? traceId = null)
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
            return new ObjectResult(new ApiProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Status = rsp.StatusCode,
                Title = rsp.Failure.Reason,
                TraceId = traceId ?? Activity.Current?.TraceId.ToString(),
                Errors = rsp.Failure.Errors?.Select(x => new ApiProblemDetailError()
                {
                    Code = x.Code,
                    Reason = x.Message,
                    Name = x.PropertyName
                }).ToArray()
            })
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

    private static ProblemHttpResult BuildProblemDetailsResult(Failure failure, string? traceId)
    {
        return TypedResults.Problem(new ProblemDetails
        {
            Extensions =
            {
                ["traceId"] = traceId ?? Activity.Current?.TraceId.ToString(),
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

    public static IResult ToResult(this MaySucceed rsp, string? traceId = null)
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
            return BuildProblemDetailsResult(rsp.Failure, traceId);
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return TypedResults.Created(url);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return TypedResults.Ok();

        return TypedResults.StatusCode(rsp.StatusCode);
    }
    
    public static IResult ToResult<T>(this MaySucceed<T> rsp, string? traceId = null)
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
            
            return BuildProblemDetailsResult(rsp.Failure, traceId);
        }

        if (rsp.StatusCode == HttpResult.HttpStatusCodeCreated)
        {
            var url = rsp.ResourceUrl();

            return TypedResults.Created(url, rsp.Value);
        }

        if(rsp.StatusCode == HttpResult.HttpStatusCodeOk) return TypedResults.Ok(rsp.Value);

        return TypedResults.StatusCode(rsp.StatusCode);
    }
}