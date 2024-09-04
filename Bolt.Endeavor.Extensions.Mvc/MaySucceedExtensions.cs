namespace Bolt.Endeavor.Extensions.Mvc;

public static class MaySucceedExtensions
{
    private const string TraceIdKey = "__trace_id__";
    internal static void TraceId(this Dictionary<string,object> source, string traceId)
    {
        source[TraceIdKey] = traceId;
    }
    
    internal static string TraceId(this MaySucceed source)
    {
        return source.ReadMetaData<string>(TraceIdKey) ?? string.Empty;
    }
    
    internal static string TraceId<T>(this MaySucceed<T> source)
    {
        return source.ReadMetaData<T,string>(TraceIdKey) ?? string.Empty;
    }
    
    private const string TenantKey = "__current_tenant__";
    internal static void Tenant(this Dictionary<string,object> source, string tenant)
    {
        source[TenantKey] = tenant;
    }
    
    internal static string Tenant(this MaySucceed source)
    {
        return source.ReadMetaData<string>(TenantKey) ?? string.Empty;
    }
    
    internal static string Tenant<T>(this MaySucceed<T> source)
    {
        return source.ReadMetaData<T,string>(TenantKey) ?? string.Empty;
    }
    
    private const string CurrentUserKey = "__current_user__";
    internal static void CurrentUser(this Dictionary<string,object> source, CurrentUser user)
    {
        source[CurrentUserKey] = user;
    }
    
    internal static CurrentUser? CurrentUser(this MaySucceed source)
    {
        return source.ReadMetaData<CurrentUser>(CurrentUserKey);
    }
    
    internal static CurrentUser? CurrentUser<T>(this MaySucceed<T> source)
    {
        return source.ReadMetaData<T,CurrentUser>(CurrentUserKey);
    }
}