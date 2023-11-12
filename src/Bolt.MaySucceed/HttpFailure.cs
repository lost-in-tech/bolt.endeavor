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
}