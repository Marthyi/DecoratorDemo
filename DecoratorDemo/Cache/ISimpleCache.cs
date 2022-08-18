namespace DecoratorDemo
{

    public interface ISimpleCache
    {
        TValue GetOrAdd<TValue>(object key, Func<TValue> add);
        Task<TValue> GetOrAddAsync<TValue>(object key, Func<Task<TValue>> add);
    }
}