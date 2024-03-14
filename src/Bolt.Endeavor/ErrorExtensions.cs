namespace Bolt.Endeavor;

public static class ErrorExtensions
{
    public static MaySucceed ToMaySucceed(this Error error) => new(new Failure(new []{error}));
    public static Task<MaySucceed> ToMaySucceedTask(this Error error) => Task.FromResult(error.ToMaySucceed());
    public static MaySucceed ToMaySucceed(this Error[] errors) => new(new Failure(errors));
    public static Task<MaySucceed> ToMaySucceedTask(this Error[] errors) => Task.FromResult(errors.ToMaySucceed());
    
    public static MaySucceed<T> ToMaySucceed<T>(this Error error) => new(new Failure(new []{error}));
    public static Task<MaySucceed<T>> ToMaySucceedTask<T>(this Error error) => Task.FromResult(error.ToMaySucceed<T>());
    public static MaySucceed<T> ToMaySucceed<T>(this Error[] errors) => new(new Failure(errors));
    public static Task<MaySucceed<T>> ToMaySucceedTask<T>(this Error[] errors) => Task.FromResult(errors.ToMaySucceed<T>());
}