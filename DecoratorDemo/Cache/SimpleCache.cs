namespace DecoratorDemo;

public class SimpleCache : ISimpleCache
{
    private IDictionary<object, object> _cache = new Dictionary<object, object>();

    public TValue GetOrAdd<TValue>(object key, Func<TValue> func)
    {
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = func();
        }

        return (TValue)_cache[key];
    }

    public async Task<TValue> GetOrAddAsync<TValue>(object key, Func<Task<TValue>> func)
    {
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = await func();
        }

        return (TValue)_cache[key];
    }
}
