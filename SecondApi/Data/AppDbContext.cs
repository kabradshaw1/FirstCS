// `using` imports a namespace so we can reference its types without the full path.
// Without this, we'd have to write `Microsoft.EntityFrameworkCore.DbContext` everywhere.
using Microsoft.EntityFrameworkCore;
// Brings in our User model so DbSet<User> can reference it below.
using SecondApi.Models;

// This class lives in the SecondApi.Data namespace — a convention that groups
// all database-related code together.
namespace SecondApi.Data;

// Entity Framework Core (EF Core) is a library that lets C# code interact with a database
// using objects instead of raw SQL. A `DbContext` is the central class in EF Core —
// it tracks changes to your objects and translates them into SQL queries.
//
// `: DbContext` is C# inheritance syntax — AppDbContext inherits all of EF Core's
// database management behavior from the base DbContext class.
//
// How this class connects to the rest of the app:
//   - Program.cs registers it via `builder.Services.AddDbContext<AppDbContext>(...)`,
//     telling EF Core which database to connect to.
//   - UserService (Services/UserService.cs) receives an AppDbContext via constructor
//     injection so it can query and modify the database.
public class AppDbContext : DbContext
{
    // `DbContextOptions<AppDbContext>` carries configuration like the connection string
    // and the database provider (Npgsql/PostgreSQL in this case).
    // ASP.NET Core's Dependency Injection (DI) container creates this options object
    // automatically based on what we registered in Program.cs, then passes it here.
    // `: base(options)` forwards those options up to the parent DbContext class.
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // `DbSet<User>` is EF Core's in-memory representation of the Users database table.
    // Each `User` object in this set corresponds to one row in the table.
    // EF Core uses the property name ("Users") to determine the table name by convention.
    // Through this property, UserService can call .ToListAsync(), .Add(), etc.
    public DbSet<User> Users { get; set; }
}
