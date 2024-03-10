using System.Net;

namespace Bolt.MaySucceed;

public static class HttpFailure
{
    public static Failure New(HttpStatusCode statusCode, string msg)
        => New(statusCode, msg, null);
    public static Failure New(HttpStatusCode statusCode, string msg, Error[]? errors) =>
        new Failure((int)statusCode, msg, errors);

    public static Failure NotFound(string msg = "Resource not found") => New(HttpStatusCode.NotFound, msg, null);
    public static Failure FailedDependency(string msg = "Dependency failed") => New(HttpStatusCode.FailedDependency, msg, null);
    public static Failure Unauthorized(string msg = "Unauthorized") => New(HttpStatusCode.Unauthorized, msg, null);
    public static Failure Forbidden(string msg = "Forbidden") => New(HttpStatusCode.Forbidden, msg, null);
    public static Failure InternalServerError(string msg = "InternalServerError") => New(HttpStatusCode.InternalServerError, msg, null);
    
    public static Failure BadRequest(string reason, Error[] errors) => New(HttpStatusCode.BadRequest, reason, errors);
    public static Failure BadRequest(params Error[] errors) => BadRequest("Please check error(s)", errors);
    public static Failure BadRequest(string reason, Error error) => BadRequest(reason, new[]{error});
    public static Failure BadRequest(Error error) => BadRequest(new[]{error});

    public static Failure Redirect(string redirectUrl, bool isPermanent, string? reason = null) 
        => new(isPermanent ? 301 : 302, reason ?? "Redirect requested")
        {
            MetaData = new Dictionary<string, object>
            {
                ["RedirectUrl"] = redirectUrl
            }
        };

    public static bool IsNotFound(Failure failure) => failure.StatusCode == 404;

    public static bool IsRedirect(Failure failure) => failure.StatusCode == 301 || failure.StatusCode == 302;

    public static bool TryGetRedirectUrl(Failure failure, out string? redirectUrl)
    {
        redirectUrl = null;
        
        if (failure.MetaData == null) return false;

        if (failure.MetaData.TryGetValue("RedirectUrl", out var url))
        {
            redirectUrl = url.ToString();
            return true;
        }

        return false;
    }
}