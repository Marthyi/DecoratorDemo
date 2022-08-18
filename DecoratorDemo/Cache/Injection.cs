using Microsoft.Extensions.DependencyInjection;

namespace DecoratorDemo.Cache;

public static class Injection
{
    public static void AddSimpleCache(this IServiceCollection services, ServiceLifetime cacheLifeTime)
    {
        services.Add(new ServiceDescriptor(typeof(ISimpleCache), typeof(SimpleCache), cacheLifeTime));
    }
}