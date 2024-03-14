namespace Bolt.Endeavor;

public static partial class MaySucceedExtensions
{
    #region MaySucceed
    
    public static MaySucceed Then(this MaySucceed source, 
        Func<MaySucceed> func)
    {
        return source.IsFailed 
                ? source 
                : func.Invoke();
    }
    
    public static Task<MaySucceed> Then(this MaySucceed source, 
        Func<CancellationToken,Task<MaySucceed>> funcAsync, 
        CancellationToken ct = default)
    {
        return source.IsFailed 
                ? Task.FromResult(source) 
                : funcAsync.Invoke(ct);
    }
    
    public static MaySucceed<T> Then<T>(this MaySucceed source, 
        Func<MaySucceed<T>> func)
    {
        return source.IsFailed 
            ? source.Failure 
            : func.Invoke();
    }
    
    
    public static Task<MaySucceed<T>> Then<T>(this MaySucceed source, 
        Func<CancellationToken,Task<MaySucceed<T>>> funcAsync, 
        CancellationToken ct = default)
    {
        return source.IsFailed 
            ? Task.FromResult(source.Failure.ToMaySucceed<T>()) 
            : funcAsync.Invoke(ct);
    }
    
    public static MaySucceed Then(this MaySucceed source, 
        Action func)
    {
        if (source.IsSucceed)
        {
            func.Invoke();
        }
        return source;
    }
    
    public static async Task<MaySucceed> Then(this MaySucceed source, 
        Func<CancellationToken,Task> funcAsync, 
        CancellationToken ct = default)
    {
        if (source.IsSucceed)
        {
            await funcAsync.Invoke(ct);
        }

        return source;
    }
    
    #endregion

    #region Task<MaySucceed>

    public static async Task<MaySucceed> Then(this Task<MaySucceed> src, 
        Func<MaySucceed> func)
    {
        var source = await src.ConfigureAwait(false);
        
        return source.IsFailed 
            ? source 
            : func.Invoke();
    }
    
    public static async Task<MaySucceed> Then(this Task<MaySucceed> src, 
        Func<CancellationToken,Task<MaySucceed>> funcAsync, 
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        return source.IsFailed 
            ? source 
            : await funcAsync.Invoke(ct);
    }
    
    public static async Task<MaySucceed<T>> Then<T>(this Task<MaySucceed> src, 
        Func<MaySucceed<T>> func)
    {
        var source = await src.ConfigureAwait(false);
        
        return source.IsFailed 
            ? source.Failure 
            : func.Invoke();
    }
    
    
    public static async Task<MaySucceed<T>> Then<T>(this Task<MaySucceed> src, 
        Func<CancellationToken,Task<MaySucceed<T>>> funcAsync, 
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        return source.IsFailed 
            ? source.Failure 
            : await funcAsync.Invoke(ct);
    }
    
    public static async Task<MaySucceed> Then(this Task<MaySucceed> src, 
        Action func)
    {
        var source = await src.ConfigureAwait(false);
        
        if (source.IsSucceed)
        {
            func.Invoke();
        }
        return source;
    }
    
    public static async Task<MaySucceed> Then(this Task<MaySucceed> src, 
        Func<CancellationToken,Task> funcAsync, 
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        
        if (source.IsSucceed)
        {
            await funcAsync.Invoke(ct);
        }

        return source;
    }

    #endregion
}