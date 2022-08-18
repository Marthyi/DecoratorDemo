using Castle.DynamicProxy;
using System.Diagnostics;

namespace DecoratorDemo.Interceptor;

internal class LoggingInterceptor : AsyncInterceptorBase
{

    public LoggingInterceptor()
    {
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        await proceed(invocation, proceedInfo);
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {

        Stopwatch sw = Stopwatch.StartNew();

        var result = await proceed(invocation, proceedInfo);

        sw.Stop();
        Console.WriteLine($"Method [{invocation.Method.Name}] took: {sw.Elapsed.TotalMilliseconds:2}ms");

        return result;
    }
}
