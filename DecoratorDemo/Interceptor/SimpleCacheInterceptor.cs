using Castle.DynamicProxy;

namespace DecoratorDemo.Interceptor;

internal class SimpleCacheInterceptor : AsyncInterceptorBase
{
    private readonly ISimpleCache _requestCache;

    public SimpleCacheInterceptor(ISimpleCache requestCache)
    {
        _requestCache = requestCache;
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        throw new NotImplementedException();
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        if (invocation.Method.GetCustomAttributes(false).Any(p => p.GetType() == typeof(ToCacheAttribute)))
        {
            return await CacheMethod(invocation, proceedInfo, proceed);
        }

        return await proceed(invocation, proceedInfo);
    }

    private Task<TResult> CacheMethod<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        var parameters = invocation.Method.GetParameters()
            .Select(p => new
            {
                parameter = p,
                CacheKey = p.CustomAttributes.FirstOrDefault(at => at.AttributeType == typeof(CacheKeyAttribute))
            })
            .Where(p => p.CacheKey != null)
            .OrderBy(p => p.parameter.Position)
            .ToArray();

        var key = parameters.Length switch
        {
            0 => null,
            1 => invocation.Arguments[parameters[0].parameter.Position],
            _ => string.Join('-', parameters.Select(p => invocation.Arguments[p.parameter.Position]))
        };

        return _requestCache.GetOrAddAsync(key, async () =>
        {
            return await proceed(invocation, proceedInfo);
        });
    }
}
