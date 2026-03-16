public class UserService
{
    private static List<User> users = new()
    {
        new User("Kyle", 30),
        new User("Alice", 25)
    };

    public List<User> GetUsers()
    {
        return users;
    }

    public void  AddUser(User user)
    {
        users.Add(user);
    }
}