namespace Bolt.MaySucceed;

public static partial class MaySucceedExtensions
{
    #region MaySucceed
    
    public static MaySucceed Otherwise(this MaySucceed src, Action<Failure> action)
    {
        if(src.IsFailed) action.Invoke(src.Failure);
        return src;
    }
    
    public static MaySucceed Otherwise(this MaySucceed src, Func<Failure,MaySucceed> alternative)
    {
        return src.IsFailed 
            ? alternative.Invoke(src.Failure) 
            : src;
    }
    
    public static async Task<MaySucceed> Otherwise(this MaySucceed src, 
        Func<Failure, CancellationToken, Task> action, 
        CancellationToken ct = default)
    {
        if (src.IsFailed)
        {
            await action.Invoke(src.Failure, ct);
        }
        
        return src;
    }
    
    public static Task<MaySucceed> Otherwise(this MaySucceed src, 
        Func<Failure, CancellationToken, Task<MaySucceed>> alternative, 
        CancellationToken ct = default)
    {
        return src.IsFailed 
            ? alternative.Invoke(src.Failure, ct) 
            : Task.FromResult(src);
    }
    
    #endregion
    
    #region Task<MaySucceed>
    
    public static async Task<MaySucceed> Otherwise(this Task<MaySucceed> source, Action<Failure> action)
    {
        var src = await source.ConfigureAwait(false);
        
        if(src.IsFailed) action.Invoke(src.Failure);
        
        return src;
    }
    
    public static async Task<MaySucceed> Otherwise(this Task<MaySucceed> source, Func<Failure,MaySucceed> alternative)
    {
        var src = await source.ConfigureAwait(false);
        
        return src.IsFailed 
            ? alternative.Invoke(src.Failure) 
            : src;
    }
    
    public static async Task<MaySucceed> Otherwise(this Task<MaySucceed> source, 
        Func<Failure, CancellationToken, Task> action, 
        CancellationToken ct = default)
    {
        var src = await source.ConfigureAwait(false);
        
        if (src.IsFailed)
        {
            await action.Invoke(src.Failure, ct);
        }
        
        return src;
    }
    
    public static async Task<MaySucceed> Otherwise(this Task<MaySucceed> source, 
        Func<Failure, CancellationToken, Task<MaySucceed>> alternative, 
        CancellationToken ct = default)
    {
        var src = await source.ConfigureAwait(false);
        return src.IsFailed 
            ? await alternative.Invoke(src.Failure, ct) 
            : src;
    }
    
    #endregion
}