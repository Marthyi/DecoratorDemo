namespace DecoratorDemo.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserById(string dummyParam, int id);

        Task<User> GetUserByName(string name, string nickname);
    }
}