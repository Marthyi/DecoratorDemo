using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DecoratorDemo.Interceptor;

public static class Injection
{
    public static IServiceCollection Intercept<TInterface, TInterceptor>(this IServiceCollection services)
        where TInterceptor : class, IAsyncInterceptor
    {

        if (!typeof(TInterface).IsInterface)
        {
            throw new ArgumentException($"{typeof(TInterface).Name} must be an interface");
        }

        services.TryAddSingleton<ProxyGenerator>();
        services.AddScoped<TInterceptor>();

        services.Decorate<TInterface>((instance, sp) =>
        {
            var proxy = sp.GetService<ProxyGenerator>();
            TInterceptor cacheInterceptor = sp.GetService<TInterceptor>();

            return (TInterface)proxy.CreateInterfaceProxyWithTargetInterface(typeof(TInterface), instance, cacheInterceptor);
        });

        return services;
    }
}
