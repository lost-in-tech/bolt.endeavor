namespace Bolt.Endeavor;

public static partial class MaySucceedExtensions
{
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static WhenCondition When(this MaySucceed source, Func<MaySucceed, bool> condition) =>
        new(source, condition);
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static WhenCondition When(this MaySucceed source, Func<Failure, bool> condition) =>
        new(source, ms => ms.IsFailed && condition.Invoke(ms.Failure));
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenCondition<T> When<T>(this MaySucceed<T> source, Func<MaySucceed<T>, bool> condition) =>
        new(source, condition);
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenCondition<T> When<T>(this MaySucceed<T> source, Func<T, bool> condition) =>
        new(source, (ms) => ms.IsSucceed && condition.Invoke(ms.Value));
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenCondition<T> When<T>(this MaySucceed<T> source, Func<Failure, bool> condition) =>
        new(source, (ms) => ms.IsFailed && condition.Invoke(ms.Failure));
    
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static WhenConditionTask When(this Task<MaySucceed> source, Func<MaySucceed, bool> condition) =>
        new(source, condition);
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static WhenConditionTask When(this Task<MaySucceed> source, Func<Failure, bool> condition) =>
        new(source, ms => ms.IsFailed && condition.Invoke(ms.Failure));
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenConditionTask<T> When<T>(this Task<MaySucceed<T>> source, Func<MaySucceed<T>, bool> condition) =>
        new(source, condition);
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenConditionTask<T> When<T>(this Task<MaySucceed<T>> source, Func<T, bool> condition) =>
        new(source, ms => ms.IsSucceed && condition.Invoke(ms.Value));
    /// <summary>
    /// Check for the condition. Allow you to execute next step `Then` when condition is true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenConditionTask<T> When<T>(this Task<MaySucceed<T>> source, Func<Failure, bool> condition) =>
        new(source, ms => ms.IsFailed && condition.Invoke(ms.Failure));
    
    /// <summary>
    /// Allow you to execute next step `Then` when current value indicates Failure
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static WhenCondition WhenFailed(this MaySucceed source) =>
        new(source, static ms => ms.IsFailed);
    /// <summary>
    /// Allow you to execute next step `Then` when current value indicates Failure
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenCondition<T> WhenFailed<T>(this MaySucceed<T> source) =>
        new(source, static ms => ms.IsFailed);
    /// <summary>
    /// Allow you to execute next step `Then` when current value indicates Failure
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static WhenConditionTask WhenFailed(this Task<MaySucceed> source) =>
        new(source, static ms => ms.IsFailed);
    /// <summary>
    /// Allow you to execute next step `Then` when current value indicates Failure
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WhenConditionTask<T> WhenFailed<T>(this Task<MaySucceed<T>> source) =>
        new(source, static ms => ms.IsFailed);
}