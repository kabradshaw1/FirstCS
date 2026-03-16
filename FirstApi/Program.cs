var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// This is the first example endpoint.  When the server runs, putting 
// http://localhost:5000/hello in the browser will return the string "Hello Kyle".  
app.MapGet("/hello", () => "Hello Kyle");

// The second example endpoint is a parameterized version of the first.  
// Putting http://localhost:5000/hello/Kyle in the browser will return the 
// same result as the first endpoint, but putting http://localhost:5000/hello/Alice 
// will return "Hello Alice".
app.MapGet("/hello/{name}", (string name) => $"Hello {name}");

// This example servers to show how to return a JSON object.  
// Putting http://localhost:5000/user in the browser will return 
// a JSON object with three properties: id, name, and role.
// still no logic here, just returning a hard coded object, 
// but you can imagine how this could be used to return a 
// user object from a database or some other source.
app.MapGet("/user", () =>
{
    return new
    {
        id = 1,
        name = "Kyle",
        role = "developer"
    };
});

// This example shows the use of query parameters.  Putting http://localhost:5000/add?a=5&b=10
//  in the browser will return 15.  This is another way to pass information to the database,
// instead of putting them in a json.  It also starts to demonstrate some of the syntax of C#, 
// such as lambda expressions and type inference.  The parameters a and b are automatically 
// parsed from the query string and passed to the lambda function, which returns their sum.
app.MapGet("/add", (int a, int b) => a + b);


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
