using System.Net;

namespace Bolt.Endeavor;

public static class HttpResult
{
    public const int HttpStatusCodeOk = 200;
    public const int HttpStatusCodeNoContent = 204;
    public const int HttpStatusCodeCreated = 301;
    public const int HttpStatusCodeAccepted = 202;
    
    
    public const int HttpStatusCodeNotFound = 404;
    public const int HttpStatusCodeBadRequest = 400;
    public const int HttpStatusCodeTempRedirect = 307;
    public const int HttpStatusCodePermRedirect = 308;

    private const string MetaDataResourceUrl = "__ResourceUrl__";

    /// <summary>
    /// Resource url of the item created. Generally available when status is created.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string? ResourceUrl<T>(this MaySucceed<T> source)
        => ReadMetaData<T,string>(source, MetaDataResourceUrl);

    public static string? ResourceUrl(this MaySucceed source)
        => ReadMetaData<string>(source, MetaDataResourceUrl);
    
    
    public static string? RedirectUrl<T>(this MaySucceed<T> source)
        => ReadFailureMetaData<T,string>(source, MetaDataRedirectUrlKey) 
           ?? ReadMetaData<T,string>(source, MetaDataRedirectUrlKey);

    public static string? RedirectUrl(this MaySucceed source)
        => ReadFailureMetaData<string>(source, MetaDataRedirectUrlKey) 
           ?? ReadMetaData<string>(source, MetaDataRedirectUrlKey);

    /// <summary>
    /// Resource url of the item created. Generally available when status is created.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TMetaDataValue"></typeparam>
    /// <returns></returns>
    public static TMetaDataValue? ReadMetaData<T,TMetaDataValue>(this MaySucceed<T> source, string name)
    {
        if (source.MetaData == null) return default;

        if (source.MetaData.TryGetValue(name, out var value)) return (TMetaDataValue)value;

        return default;
    }
    
    public static T? ReadMetaData<T>(this MaySucceed source, string name)
    {
        if (source.MetaData == null) return default;

        if (source.MetaData.TryGetValue(name, out var value)) return (T)value;

        return default;
    }
    
    public static TMetaDataValue? ReadFailureMetaData<T,TMetaDataValue>(this MaySucceed<T> source, string name)
    {
        if (source.Failure?.MetaData == null) return default;

        if (source.Failure.MetaData.TryGetValue(name, out var value)) return (TMetaDataValue)value;

        return default;
    }
    
    public static T? ReadFailureMetaData<T>(this MaySucceed source, string name)
    {
        if (source.Failure?.MetaData == null) return default;

        if (source.Failure.MetaData.TryGetValue(name, out var value)) return (T)value;

        return default;
    }

    public static MaySucceed<T> Created<T>(T value, string? uri)
    {
        var created = MaySucceed.Ok(HttpStatusCodeCreated, value);

        if (!string.IsNullOrWhiteSpace(uri))
        {
            return created with
            {
                MetaData = new Dictionary<string, object>
                {
                    [MetaDataResourceUrl] = uri
                }
            };
        }

        return created;
    }

    public static MaySucceed<T> Created<T>(T value) => Created(value, null);

    public static MaySucceed Created() => MaySucceed.Ok(HttpStatusCodeCreated);

    public static MaySucceed<T> Ok<T>(T value) => MaySucceed.Ok(HttpStatusCodeOk, value);
    
    public static MaySucceed Ok() => MaySucceed.Ok(HttpStatusCodeOk);

    public static MaySucceed Succeed(HttpStatusCode statusCode) => MaySucceed.Ok((int)statusCode);
    public static MaySucceed Succeed(HttpStatusCode statusCode, Dictionary<string,object> metaData) => MaySucceed.Ok((int)statusCode, metaData);
    
    public static MaySucceed<T> Succeed<T>(HttpStatusCode statusCode, T value) => MaySucceed.Ok((int)statusCode, value);
    public static MaySucceed<T> Succeed<T>(HttpStatusCode statusCode, T value, Dictionary<string,object> metaData) => MaySucceed<T>.Ok((int)statusCode, value, metaData);

    #region Failure

    public static Failure Failure(HttpStatusCode statusCode, string msg, Error[]? errors) =>
        new Failure((int)statusCode, msg, errors);

    public static Failure Failure(HttpStatusCode statusCode, string msg)
        => Failure(statusCode, msg, null);
    
    public static Failure NotFound(string msg = "Resource not found") => Failure(HttpStatusCode.NotFound, msg, null);

    public static Failure FailedDependency(string msg = "Dependency failed") =>
        Failure(HttpStatusCode.FailedDependency, msg, null);

    public static Failure Unauthorized(string msg = "Unauthorized") => Failure(HttpStatusCode.Unauthorized, msg, null);
    public static Failure Forbidden(string msg = "Forbidden") => Failure(HttpStatusCode.Forbidden, msg, null);

    public static Failure InternalServerError(string msg = "InternalServerError") =>
        Failure(HttpStatusCode.InternalServerError, msg, null);

    public static Failure BadRequest(string reason, Error[] errors) => Failure(HttpStatusCode.BadRequest, reason, errors);
    public static Failure BadRequest(params Error[] errors) => BadRequest("Please check error(s)", errors);
    public static Failure BadRequest(string reason, Error error) => BadRequest(reason, new[] { error });
    public static Failure BadRequest(Error error) => BadRequest(new[] { error });

    private const string MetaDataRedirectUrlKey = "RedirectUrl";

    public static Failure Redirect(string redirectUrl, bool isPermanent, string? reason = null)
        => new(isPermanent ? 301 : 302, reason ?? "Redirect requested")
        {
            MetaData = new Dictionary<string, object>
            {
                [MetaDataRedirectUrlKey] = redirectUrl
            }
        };

    public static bool IsNotFound(Failure failure) => failure.StatusCode == 404;

    public static bool IsRedirect(Failure failure) => failure.StatusCode == 301 || failure.StatusCode == 302;

    public static bool TryGetRedirectUrl(Failure failure, out string? redirectUrl)
    {
        redirectUrl = null;

        if (failure.MetaData == null) return false;

        if (failure.MetaData.TryGetValue(MetaDataRedirectUrlKey, out var url))
        {
            redirectUrl = url.ToString();
            return true;
        }

        return false;
    }

    #endregion
}