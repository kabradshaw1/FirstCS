using SecondApi.Models;
using SecondApi.Data;
using SecondApi.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=secondapi;Username=kylebradshaw"));
builder.Services.AddScoped<UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.MapPost("/users", async (User user, UserService service) =>
{
    Console.WriteLine("Endpoint hit");

    await service.AddUser(user);

    return Results.Ok(user);
});

app.MapGet("/users", async (UserService service) =>
{
    return await service.GetUsers();
});

app.MapGet("/test-service", (UserService service) =>
{
    return "Service resolved!";
});

app.MapPost("/debug", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    Console.WriteLine(body);
    return body;
});
app.Run();