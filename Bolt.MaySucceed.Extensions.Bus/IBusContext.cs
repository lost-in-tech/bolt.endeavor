namespace Bolt.MaySucceed.Extensions.Bus;

public interface IBusContext : IBusContextReader
{
    void Set<T>(string key, T? value);
}
public interface IBusContextReader
{
    T? TryGet<T>(string key);
}

internal sealed class BusContext : IBusContext
{
    private readonly Dictionary<string, object?> _store = new();

    public void Set<T>(string key, T? value)
    {
        _store[key] = value;
    }

    public T? TryGet<T>(string key)
    {
        return _store.TryGetValue(key, out var value) 
                    ? value == null 
                        ? default 
                        : (T)value 
                    : default;
    }
}