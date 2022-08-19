namespace DecoratorDemo.Repository
{
    public class UserRepository : IUserRepository
    {

        [ToCache]
        public async Task<User> GetUserById([CacheKey] string dummyParam, [CacheKey] int id)
        {
            await Task.Delay(500);
            return new User() { Name = $"{dummyParam}_{id}" };
        }

        [ToCache]
        public async Task<User> GetUserByName([CacheKey] string name,string nickname)
        {
            await Task.Delay(500);
            return new User() { Name = $"{name}_{nickname}" };
        }
    }
}