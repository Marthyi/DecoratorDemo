namespace DecoratorDemo.Repository
{
    public class UserRepository : IUserRepository
    {

        public async Task<User> GetUserById(string dummyParam, int id)
        {
            await Task.Delay(500);
            return new User() { Name = $"Bill_{id}" };
        }

        public async Task<User> GetUserByName(string name,string nickname)
        {
            await Task.Delay(500);
            return new User() { Name = $"name" };
        }
    }
}