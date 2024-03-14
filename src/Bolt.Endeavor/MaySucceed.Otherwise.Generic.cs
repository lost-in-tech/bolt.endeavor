namespace Bolt.Endeavor;

public static partial class MaySucceedExtensions
{
    #region MaySucceed<T>
    
    public static MaySucceed<T> Otherwise<T>(this MaySucceed<T> src, Action<Failure> action)
    {
        if(src.IsFailed) action.Invoke(src.Failure);
        return src;
    }
    
    public static MaySucceed<T> Otherwise<T>(this MaySucceed<T> src, Func<Failure,MaySucceed<T>> alternative)
    {
        return src.IsFailed 
            ? alternative.Invoke(src.Failure) 
            : src;
    }
    
    public static async Task<MaySucceed<T>> Otherwise<T>(this MaySucceed<T> src, 
        Func<Failure, CancellationToken, Task> action, 
        CancellationToken ct = default)
    {
        if (src.IsFailed)
        {
            await action.Invoke(src.Failure, ct);
        }
        
        return src;
    }
    
    public static Task<MaySucceed<T>> Otherwise<T>(this MaySucceed<T> src, 
        Func<Failure, CancellationToken, Task<MaySucceed<T>>> alternative, 
        CancellationToken ct = default)
    {
        return src.IsFailed 
            ? alternative.Invoke(src.Failure, ct) 
            : Task.FromResult(src);
    }
    
    #endregion
    
    #region Task<MaySucceed>
    
    public static async Task<MaySucceed<T>> Otherwise<T>(this Task<MaySucceed<T>> source, Action<Failure> action)
    {
        var src = await source.ConfigureAwait(false);
        
        if(src.IsFailed) action.Invoke(src.Failure);
        
        return src;
    }
    
    public static async Task<MaySucceed<T>> Otherwise<T>(this Task<MaySucceed<T>> source, Func<Failure,MaySucceed<T>> alternative)
    {
        var src = await source.ConfigureAwait(false);
        
        return src.IsFailed 
            ? alternative.Invoke(src.Failure) 
            : src;
    }
    
    public static async Task<MaySucceed<T>> Otherwise<T>(this Task<MaySucceed<T>> source, 
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
    
    public static async Task<MaySucceed<T>> Otherwise<T>(this Task<MaySucceed<T>> source, 
        Func<Failure, CancellationToken, Task<MaySucceed<T>>> alternative, 
        CancellationToken ct = default)
    {
        var src = await source.ConfigureAwait(false);
        return src.IsFailed 
            ? await alternative.Invoke(src.Failure, ct) 
            : src;
    }
    
    #endregion
}