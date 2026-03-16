using SecondApi.Models;
using SecondApi.Data;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddUser(User user)
    {
        Console.WriteLine($"Adding {user.Name}");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}