// This namespace groups all service classes together.
// Placing the `namespace` declaration at the top (file-scoped) applies it to the whole file,
// which is the modern C# 10+ style — no need for wrapping curly braces.
namespace SecondApi.Services;

// Bring in the User model so methods can accept and return User objects.
using SecondApi.Models;
// Bring in AppDbContext so this service can talk to the database.
using SecondApi.Data;
// Provides ToListAsync() and other async EF Core extension methods.
using Microsoft.EntityFrameworkCore;

// A "service" class is where business logic lives. Keeping logic here (rather than
// directly in the route handlers in Program.cs) makes the code easier to test and reuse.
// Program.cs registers this class with the DI container via `builder.Services.AddScoped<UserService>()`,
// so ASP.NET Core can automatically inject it into route handler lambdas.
public class UserService
{
    // `private` means this field is only accessible within this class.
    // `readonly` means it can only be assigned once — in the constructor — and never changed after.
    // Together, `private readonly` is the standard pattern for injected dependencies in C#,
    // because a service should hold a stable reference to its collaborators.
    private readonly AppDbContext _context;

    // This is the constructor. When ASP.NET Core's Dependency Injection (DI) container
    // creates a UserService, it looks at this constructor's parameters, sees AppDbContext,
    // finds the registered AppDbContext from `builder.Services.AddDbContext<AppDbContext>(...)`,
    // and passes it in automatically. This is called "constructor injection."
    public UserService(AppDbContext context)
    {
        // Store the injected context so all methods in this class can use it.
        _context = context;
    }

    // `async` marks this method as asynchronous — it can pause while waiting for I/O
    // (like a database query) without blocking the server thread.
    // `Task<List<User>>` is the return type: a Task wraps the eventual result (List<User>),
    // similar to a Promise in JavaScript. `await` unwraps the result when it's ready.
    public async Task<List<User>> GetUsers()
    {
        // `_context.Users` accesses the DbSet<User> defined in AppDbContext.
        // `.ToListAsync()` tells EF Core to generate a SELECT * FROM "Users" SQL query,
        // execute it against PostgreSQL, and return the rows as a List<User>.
        // The `await` keyword suspends this method until the database responds.
        return await _context.Users.ToListAsync();
    }

    // `Task` (without a type parameter) means this async method returns nothing (like `void`),
    // but callers can still `await` it to know when it's finished.
    public async Task AddUser(User user)
    {
        // $"..." is C# string interpolation — embeds expressions inside a string literal.
        Console.WriteLine($"Adding {user.Name}");

        // `.Add(user)` stages the new User object in EF Core's change tracker.
        // At this point no SQL has been sent — EF Core just knows about the pending insert.
        _context.Users.Add(user);

        // `SaveChangesAsync()` flushes all pending changes to the database in a single transaction.
        // Here it executes an INSERT INTO "Users" (...) VALUES (...) SQL statement.
        // Without this call, the user would never actually be saved.
        await _context.SaveChangesAsync();
    }
}
