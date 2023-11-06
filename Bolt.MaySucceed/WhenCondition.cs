namespace Bolt.MaySucceed;

public class WhenCondition
{
    private readonly MaySucceed _source;
    private readonly Func<MaySucceed, bool> _condition;

    public WhenCondition(MaySucceed source, Func<MaySucceed,bool> condition)
    {
        _source = source;
        _condition = condition;
    }
    
    #region action

    public MaySucceed Then(Action<MaySucceed> action)
    {
        if (_condition.Invoke(_source))
        {
            action.Invoke(_source);
        }

        return _source;
    }
    
    public async Task<MaySucceed> Then(Func<MaySucceed, CancellationToken, Task> action, CancellationToken ct = default)
    {
        if (_condition.Invoke(_source))
        {
            await action.Invoke(_source, ct);
        }

        return _source;
    }
    
    #endregion

    #region func
    
    public MaySucceed Then(Func<MaySucceed, MaySucceed> func)
    {
        if (_condition.Invoke(_source)) return func.Invoke(_source);
        return _source;
    }
    
    public Task<MaySucceed> Then(Func<MaySucceed, CancellationToken, Task<MaySucceed>> func, 
        CancellationToken ct = default)
    {
        if (_condition.Invoke(_source)) return func.Invoke(_source, ct);
        return Task.FromResult(_source);
    }
    
    #endregion
}

public class WhenCondition<T>
{
    private readonly MaySucceed<T> _source;
    private readonly Func<MaySucceed<T>, bool> _condition;

    public WhenCondition(MaySucceed<T> source, Func<MaySucceed<T>,bool> condition)
    {
        _source = source;
        _condition = condition;
    }
    
    #region action

    public MaySucceed<T> Then(Action<MaySucceed<T>> action)
    {
        if (_condition.Invoke(_source))
        {
            action.Invoke(_source);
        }

        return _source;
    }
    
    public async Task<MaySucceed<T>> Then(Func<MaySucceed<T>, CancellationToken, Task> action, CancellationToken ct = default)
    {
        if (_condition.Invoke(_source))
        {
            await action.Invoke(_source, ct);
        }

        return _source;
    }
    
    #endregion

    #region func
    
    public MaySucceed<T> Then(Func<MaySucceed<T>, MaySucceed<T>> func)
    {
        if (_condition.Invoke(_source)) return func.Invoke(_source);
        return _source;
    }
    
    public Task<MaySucceed<T>> Then(Func<MaySucceed<T>, CancellationToken, Task<MaySucceed<T>>> func, 
        CancellationToken ct = default)
    {
        if (_condition.Invoke(_source)) return func.Invoke(_source, ct);
        return Task.FromResult(_source);
    }
    
    #endregion
}