using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DecoratorDemo.Interceptor;

public static class Injection
{
    public static IServiceCollection Intercept<TInterface>(this IServiceCollection services, params Type[] interceptorsTypes)
    {
        if (!typeof(TInterface).IsInterface)
        {
            throw new ArgumentException($"{typeof(TInterface).Name} must be an interface");
        }

        var descriptor = services.FirstOrDefault(p => p.ServiceType == typeof(TInterface));

        if (descriptor == null)
        {
            return services;
        }

        services.TryAddSingleton<ProxyGenerator>();

        services.Remove(descriptor);
        services.TryAddScoped(descriptor.ImplementationType);

        foreach (var interceptorType in interceptorsTypes)
        {
            services.TryAddScoped(interceptorType);
        }

        services.AddScoped(typeof(TInterface), sp =>
        {
            var proxy = sp.GetService<ProxyGenerator>();
            var instance = sp.GetService(descriptor.ImplementationType);

            var interceptors = interceptorsTypes
                                        .Select(t => sp.GetService(t))
                                        .Cast<IAsyncInterceptor>()
                                        .ToArray();

            return (TInterface)proxy.CreateInterfaceProxyWithTargetInterface(typeof(TInterface), instance, interceptors);
        });

        return services;
    }
}