using SecondApi.Models;
using SecondApi.Data;
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

app.UseHttpsRedirection();

app.MapPost("/users", async (User user, UserService service) => 
{
    await service.AddUser(user);
    return Results.Created("/users", user);
});

app.MapGet("/users", async (UserService service) =>
{
    return await service.GetUsers();
});

app.Run();