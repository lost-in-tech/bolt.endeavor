namespace Bolt.MaySucceed;

public static partial class MaySucceedExtensions
{
    #region MaySucceed<T>
    
    public static MaySucceed Then<T>(
        this MaySucceed<T> source, 
        Func<T,MaySucceed> func)
    {
        return source.IsFailed 
            ? source.Failure 
            : func.Invoke(source.Value);
    }
    
    public static MaySucceed<TOutput> Then<T,TOutput>(
        this MaySucceed<T> source, 
        Func<T,MaySucceed<TOutput>> func)
    {
        return source.IsFailed 
            ? source.Failure 
            : func.Invoke(source.Value);
    }
    
    public static MaySucceed<T> Then<T>(
        this MaySucceed<T> source, 
        Action<T> action)
    {
        if (source.IsSucceed)
        {
            action.Invoke(source.Value);
        }

        return source;
    }
    
    public static Task<MaySucceed> Then<T>(
        this MaySucceed<T> source, 
        Func<T, CancellationToken, Task<MaySucceed>> func, 
        CancellationToken ct = default)
    {
        return source.IsFailed 
            ? source.Failure.ToMaySucceedTask() 
            : func.Invoke(source.Value, ct);
    }
    
    public static Task<MaySucceed<TOutput>> Then<T,TOutput>(
        this MaySucceed<T> source, 
        Func<T, CancellationToken, Task<MaySucceed<TOutput>>> func,
        CancellationToken ct = default)
    {
        return source.IsFailed 
            ? source.Failure.ToMaySucceedTask<TOutput>() 
            : func.Invoke(source.Value, ct);
    }
    
    public static Task<MaySucceed<T>> Then<T>(this MaySucceed<T> source, Func<T, 
            CancellationToken, Task> action,
            CancellationToken ct = default)
    {
        if (source.IsSucceed)
        {
            action.Invoke(source.Value, ct);
        }

        return Task.FromResult(source);
    }
    
    #endregion
    
    #region Task<MaySucceed<T>>
    
    public static async Task<MaySucceed> Then<T>(
        this Task<MaySucceed<T>> src, 
        Func<T,MaySucceed> func)
    {
        var source = await src.ConfigureAwait(false);
        return source.IsFailed 
            ? source.Failure 
            : func.Invoke(source.Value);
    }
    
    public static async Task<MaySucceed<TOutput>> Then<T,TOutput>(
        this Task<MaySucceed<T>> src, 
        Func<T,MaySucceed<TOutput>> func)
    {
        var source = await src.ConfigureAwait(false);
        return source.IsFailed 
            ? source.Failure 
            : func.Invoke(source.Value);
    }
    
    public static async Task<MaySucceed<T>> Then<T>(
        this Task<MaySucceed<T>> src, 
        Action<T> action)
    {
        var source = await src.ConfigureAwait(false);
        
        if (source.IsSucceed)
        {
            action.Invoke(source.Value);
        }

        return source;
    }
    
    public static async Task<MaySucceed> Then<T>(
        this Task<MaySucceed<T>> src, 
        Func<T, CancellationToken, Task<MaySucceed>> func, 
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        
        return source.IsFailed 
            ? source.Failure 
            : await func.Invoke(source.Value, ct);
    }
    
    public static async Task<MaySucceed<TOutput>> Then<T,TOutput>(
        this Task<MaySucceed<T>> src, 
        Func<T, CancellationToken, Task<MaySucceed<TOutput>>> func,
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        
        return source.IsFailed 
            ? source.Failure 
            : await func.Invoke(source.Value, ct);
    }
    
    public static async Task<MaySucceed<T>> Then<T>(
        this Task<MaySucceed<T>> src, 
        Func<T,CancellationToken, Task> action,
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        
        if (source.IsSucceed)
        {
            await action.Invoke(source.Value, ct);
        }

        return source;
    }
    
    #endregion
}