namespace Bolt.Endeavor;

public static partial class MaySucceedExtensions
{
    public static MaySucceed<TOutput> MapTo<TInput, TOutput>(
        this MaySucceed<TInput> src, 
        Func<TInput, TOutput> func)
    {
        if (src.IsFailed) return src.Failure;
        return func.Invoke(src.Value);
    }
    
    public static async Task<MaySucceed<TOutput>> MapTo<TInput, TOutput>(
        this MaySucceed<TInput> src, 
        Func<TInput, CancellationToken, Task<TOutput>> func,
        CancellationToken ct = default)
    {
        if (src.IsFailed) return src.Failure;
        return await func.Invoke(src.Value, ct);
    }
    
    public static async Task<MaySucceed<TOutput>> MapTo<TInput, TOutput>(
        this Task<MaySucceed<TInput>> src, 
        Func<TInput, TOutput> func)
    {
        var source = await src.ConfigureAwait(false);
        if (source.IsFailed) return source.Failure;
        return func.Invoke(source.Value);
    }
    
    public static async Task<MaySucceed<TOutput>> MapTo<TInput, TOutput>(
        this Task<MaySucceed<TInput>> src, 
        Func<TInput, CancellationToken, Task<TOutput>> func,
        CancellationToken ct = default)
    {
        var source = await src.ConfigureAwait(false);
        if (source.IsFailed) return source.Failure;
        return await func.Invoke(source.Value, ct);
    }
}