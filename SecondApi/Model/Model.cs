// A namespace groups related classes together under a shared name, similar to a folder.
// "SecondApi.Models" means this file belongs to the Models sub-group of the SecondApi project.
// Other files can reference this class by adding `using SecondApi.Models;` at the top.
namespace SecondApi.Models;

// A class is a blueprint for creating objects. In Entity Framework Core (EF Core),
// each public class like this can map to a database table. EF Core will look at
// the property names and types to infer the table columns.
// This `User` class is used by:
//   - AppDbContext (Data/AppDbContext.cs) as a DbSet<User>, which represents the Users table
//   - UserService (Services/UserService.cs) as the parameter and return type for CRUD operations
//   - Program.cs endpoint lambdas, where ASP.NET Core deserializes incoming JSON into a User object
public class User
{
    // `{ get; set; }` is an "auto-property" — C#'s shorthand for a field with a getter and setter.
    // EF Core uses the setter to populate values when it reads rows from the database,
    // and ASP.NET Core's JSON deserializer uses it when parsing request bodies.
    // `Id` is treated as the primary key by EF Core convention (any property named "Id").
    public int Id { get; set; }

    // `= ""` sets a default value so the property is never null (C# non-nullable string requirement).
    // Without this default, the compiler would warn that `Name` might be null
    // when EF Core constructs a User object before populating it from the database.
    public string Name { get; set; } = "";

    public int Age { get; set; }
}
