using SecondApi.Models;
/* 
This is an example of simple class in C#. Classes are a fundemental building
block of object oriented programming, and they allow you to create your own data types
and define the properties and methods that they have. 
*/
public class UserService
{
    /*
        
    */
    private static List<User> users = new()
    {
        new User("Kyle", 30),
        new User("Alice", 25)
    };

    public List<User> GetUsers()
    {
        return users;
    }

    public void AddUser(User user)
    {
        users.Add(user);
    }
}