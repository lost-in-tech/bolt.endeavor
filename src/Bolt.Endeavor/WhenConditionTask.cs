namespace Bolt.Endeavor;

public class WhenConditionTask
{
    private readonly Task<MaySucceed> _source;
    private readonly Func<MaySucceed, bool> _condition;

    public WhenConditionTask(Task<MaySucceed> source, Func<MaySucceed,bool> condition)
    {
        _source = source;
        _condition = condition;
    }
    
    #region action

    public async Task<MaySucceed> Then(Action<MaySucceed> action)
    {
        var source = await _source.ConfigureAwait(false);
        if (_condition.Invoke(source))
        {
            action.Invoke(source);
        }

        return source;
    }
    
    public async Task<MaySucceed> Then(Func<MaySucceed, CancellationToken, Task> action, CancellationToken ct = default)
    {
        var source = await _source.ConfigureAwait(false);
        
        if (_condition.Invoke(source))
        {
            await action.Invoke(source, ct);
        }

        return source;
    }
    
    #endregion

    #region func
    
    public async Task<MaySucceed> Then(Func<MaySucceed, MaySucceed> func)
    {
        var source = await _source.ConfigureAwait(false);
        if (_condition.Invoke(source)) return func.Invoke(source);
        return source;
    }
    
    public async Task<MaySucceed> Then(Func<MaySucceed, CancellationToken, Task<MaySucceed>> func, 
        CancellationToken ct = default)
    {
        var source = await _source.ConfigureAwait(false);
        if (_condition.Invoke(source)) return await func.Invoke(source, ct);
        return source;
    }
    
    #endregion
}

public class WhenConditionTask<T>
{
    private readonly Task<MaySucceed<T>> _source;
    private readonly Func<MaySucceed<T>, bool> _condition;

    public WhenConditionTask(Task<MaySucceed<T>> source, Func<MaySucceed<T>,bool> condition)
    {
        _source = source;
        _condition = condition;
    }
    
    #region action

    public async Task<MaySucceed<T>> Then(Action<MaySucceed<T>> action)
    {
        var source = await _source.ConfigureAwait(false);
        
        if (_condition.Invoke(source))
        {
            action.Invoke(source);
        }

        return source;
    }
    
    public async Task<MaySucceed<T>> Then(Func<MaySucceed<T>, CancellationToken, Task> action, CancellationToken ct = default)
    {
        var source = await _source.ConfigureAwait(false);
        
        if (_condition.Invoke(source))
        {
            await action.Invoke(source, ct);
        }

        return source;
    }
    
    #endregion

    #region func
    
    public async Task<MaySucceed<T>> Then(Func<MaySucceed<T>, MaySucceed<T>> func)
    {
        var source = await _source.ConfigureAwait(false);
        if (_condition.Invoke(source)) return func.Invoke(source);
        return source;
    }
    
    public async Task<MaySucceed<T>> Then(Func<MaySucceed<T>, CancellationToken, Task<MaySucceed<T>>> func, 
        CancellationToken ct = default)
    {
        var source = await _source.ConfigureAwait(false);
        if (_condition.Invoke(source)) return await func.Invoke(source, ct);
        return source;
    }
    
    #endregion
}