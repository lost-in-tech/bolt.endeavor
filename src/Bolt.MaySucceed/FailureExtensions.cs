namespace Bolt.MaySucceed;

public static class FailureExtensions
{
    public static MaySucceed ToMaySucceed(this Failure failure) => new(failure);
    public static Task<MaySucceed> ToMaySucceedTask(this Failure failure) => Task.FromResult(new MaySucceed(failure));
    
    public static MaySucceed<T> ToMaySucceed<T>(this Failure failure) => new(failure);
    public static Task<MaySucceed<T>> ToMaySucceedTask<T>(this Failure failure) => Task.FromResult(new MaySucceed<T>(failure));
}