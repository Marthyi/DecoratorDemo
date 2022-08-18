namespace DecoratorDemo.Repository
{
    public interface IUserRepository
    {
        [ToCache]
        Task<User> GetUserById([CacheKey] string dummyParam, [CacheKey] int id);

        [ToCache]
        Task<User> GetUserByName([CacheKey] string name, string nickname);
    }
}