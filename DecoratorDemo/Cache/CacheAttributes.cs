namespace DecoratorDemo;

[AttributeUsage(AttributeTargets.Parameter)]
public class CacheKeyAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class CachePerRequestAttribute : Attribute
{
}