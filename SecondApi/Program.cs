// `using` statements at the top import namespaces so their types can be used
// without writing the full path (e.g., `User` instead of `SecondApi.Models.User`).
using SecondApi.Models;   // Gives access to the User class (Model/Model.cs)
using SecondApi.Data;     // Gives access to AppDbContext (Data/AppDbContext.cs)
using SecondApi.Services; // Gives access to UserService (Services/UserService.cs)
using Microsoft.EntityFrameworkCore; // Provides UseNpgsql() and other EF Core extension methods

// `WebApplication.CreateBuilder(args)` bootstraps the application:
// it reads configuration (appsettings.json, environment variables, command-line args),
// sets up logging, and prepares the Dependency Injection (DI) container.
var builder = WebApplication.CreateBuilder(args);

// --- Dependency Injection (DI) Container Registration ---
// `builder.Services` is the DI container. Anything registered here can be
// automatically injected into constructors and route handler parameters.

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Registers the OpenAPI (Swagger) document generator so the app can serve API docs.
builder.Services.AddOpenApi();

// `AddDbContext<AppDbContext>` registers EF Core with the DI container.
// The lambda configures which database provider to use (Npgsql = PostgreSQL)
// and passes the connection string. ASP.NET Core will create AppDbContext
// instances automatically whenever something in the app needs one.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=secondapi;Username=kylebradshaw"));

// `AddScoped<UserService>` registers UserService with "scoped" lifetime,
// meaning a new UserService instance is created for each incoming HTTP request
// and disposed when that request finishes. This is the most common lifetime
// for services that depend on a DbContext (which is also scoped).
builder.Services.AddScoped<UserService>();

// `builder.Build()` finalizes all the DI registrations and middleware configuration,
// then creates the `WebApplication` object (called `app`) that we'll use to
// define routes and run the server.
var app = builder.Build();

// --- HTTP Request Pipeline (Middleware) Configuration ---

// Configure the HTTP request pipeline.
// `IsDevelopment()` checks whether the ASPNETCORE_ENVIRONMENT variable is "Development".
// We only expose the OpenAPI/Swagger UI in development so it's not publicly accessible in production.
if (app.Environment.IsDevelopment())
{
    // Registers a route (typically /openapi/v1.json) that serves the OpenAPI spec.
    app.MapOpenApi();
}

// app.UseHttpsRedirection(); // (commented out) would redirect HTTP requests to HTTPS

// --- Route Definitions (Minimal API) ---
// `MapPost` and `MapGet` register routes with the HTTP method and path.
// The second argument is a lambda (anonymous function) that runs when the route is matched.

// POST /users — accepts a JSON body, deserializes it into a User object, and saves it.
// `async (User user, UserService service) =>` is an async lambda. ASP.NET Core automatically:
//   1. Deserializes the request body JSON into `user` (model binding)
//   2. Resolves `service` from the DI container (dependency injection into the lambda)
app.MapPost("/users", async (User user, UserService service) =>
{
    Console.WriteLine("Endpoint hit");

    // Delegates to UserService.AddUser, which stages the insert and calls SaveChangesAsync.
    await service.AddUser(user);

    // `Results.Ok(user)` returns an HTTP 200 response with the user object serialized as JSON.
    // The `Results` static class provides helper methods for common HTTP responses
    // (Results.NotFound(), Results.Created(), Results.BadRequest(), etc.).
    return Results.Ok(user);
});

// GET /users — retrieves all users from the database.
// ASP.NET Core infers the response type from the return value and serializes it to JSON.
app.MapGet("/users", async (UserService service) =>
{
    // Returns a List<User> which ASP.NET Core automatically serializes as a JSON array.
    return await service.GetUsers();
});

// GET /test-service — a simple health-check to verify UserService is being injected correctly.
// If the DI container can't resolve UserService, this request would throw a runtime error.
app.MapGet("/test-service", (UserService service) =>
{
    return "Service resolved!";
});

// POST /debug — reads the raw HTTP request body as a string and echoes it back.
// Useful for debugging what JSON payload a client is actually sending.
// `HttpContext` is injected by ASP.NET Core and provides access to the raw request/response.
app.MapPost("/debug", async (HttpContext context) =>
{
    // `StreamReader` reads the raw body stream as text.
    // `using` ensures the StreamReader is disposed (closed) after the block finishes.
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    Console.WriteLine(body);
    return body;
});

// `app.Run()` starts the web server and begins listening for incoming HTTP requests.
// This call blocks until the application is shut down (e.g., Ctrl+C).
// All the route handlers, middleware, and DI registrations configured above are now active.
app.Run();
