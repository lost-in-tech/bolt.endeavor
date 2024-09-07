using System.Diagnostics;
using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bolt.Endeavor.Extensions.Mvc;

public static class RequestBusExtensions
{
    public static async Task<IActionResult> ActionResult<TRequest, TResponse>(this IRequestBus bus, TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send<TRequest, TResponse>(request, ct);

        
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
    
    public static async Task<IActionResult> ActionResult<TRequest>(this IRequestBus bus, TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send(request, ct);

        
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
    
    public static async Task<IResult> Result<TRequest, TResponse>(this IRequestBus bus, TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send<TRequest, TResponse>(request, ct);

        
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
    
    public static async Task<IResult> Result<TRequest>(this IRequestBus bus, TRequest request, CancellationToken ct)
    {
        var rsp = await bus.Send(request, ct);

        
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