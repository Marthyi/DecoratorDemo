using DecoratorDemo.Cache;
using DecoratorDemo.Interceptor;
using DecoratorDemo.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Xunit;

namespace DecoratorDemo;

public class UnitTest1
{
    [Fact]
    public async void TestDecorator()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSimpleCache(ServiceLifetime.Scoped);



        services.AddScoped<IUserRepository, UserRepository>();
        services.Intercept<IUserRepository>(
                            typeof(SimpleCacheInterceptor),
                            typeof(LoggingInterceptor)
                            );      


        var provider = services.BuildServiceProvider();

        IUserRepository repository = provider.GetService<IUserRepository>();


        await AssertDuration(async () => await repository.GetUserById("titi", 42), p => p.TotalMilliseconds > 500);
        await AssertDuration(async () => await repository.GetUserById("tata", 42), p => p.TotalMilliseconds > 500);

        var user = await repository.GetUserById("tata", 42);
        Assert.Equal("tata_42", user.Name);

        await AssertDuration(async () => await repository.GetUserById("titi", 43), p => p.TotalMilliseconds > 500);
        await AssertDuration(async () => await repository.GetUserById("titi", 43), p => p.TotalMilliseconds < 100);

        await AssertDuration(async () => await repository.GetUserByName("paul", "a"), p => p.TotalMilliseconds > 500);
        await AssertDuration(async () => await repository.GetUserByName("paul", "a"), p => p.TotalMilliseconds < 100);
        await AssertDuration(async () => await repository.GetUserByName("paul", "p"), p => p.TotalMilliseconds < 100);
        await AssertDuration(async () => await repository.GetUserByName("pppp", "a"), p => p.TotalMilliseconds > 500);


        using var scope = provider.CreateScope();
        repository = scope.ServiceProvider.GetService<IUserRepository>();
        await AssertDuration(async () => await repository.GetUserById("titi", 42), p => p.TotalMilliseconds > 500);

    }

    private async Task AssertDuration(Func<Task> action, Func<TimeSpan, bool> predicate)
    {
        Stopwatch sw = Stopwatch.StartNew();
        await action();
        sw.Stop();

        Assert.True(predicate(sw.Elapsed));
    }
}