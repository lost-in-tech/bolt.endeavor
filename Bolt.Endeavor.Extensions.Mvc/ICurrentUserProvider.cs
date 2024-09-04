using Bolt.Endeavor.Extensions.Bus;

namespace Bolt.Endeavor.Extensions.Mvc;

public interface ICurrentUserProvider
{
    CurrentUser Get();
}

public record CurrentUser
{
    public bool IsAuthenticated { get; init; }
    public string? UserId { get; init; }
    public Dictionary<string,object>? MetaData { get; init; }
}

public static class CurrentUserExtensions
{
    private const string CurrentUserKey = "__current_user__";
    private static readonly CurrentUser DefaultUser = new CurrentUser();
    
    /// <summary>
    /// Set trace id in bus context
    /// </summary>
    /// <param name="context"></param>
    /// <param name="traceId"></param>
    public static void CurrentUser(this IBusContext context, CurrentUser traceId)
    {
        context.Set(CurrentUserKey, traceId);
    }

    /// <summary>
    /// Get trace id from bus context if available
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static CurrentUser CurrentUser(this IBusContextReader context)
    {
        return context.TryGet<CurrentUser>(CurrentUserKey) ?? DefaultUser;
    }
}