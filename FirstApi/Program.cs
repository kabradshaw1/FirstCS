
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

// This example is for a POST endpoint.  This requires the use of a record
// in C# records are placed at the end.  The User record should be created first.
// This tells the endpoint that will will expect a JSON object in the body of the request 
//that can be deserialized into a User record.
// it will allow auto completion unce user. is typed to tell you what properties are
// available.  
app.MapPost("/users", (User user) =>
{
    return $"Created user {user.Name} age {user.Age}";
});


app.Run();

// This record defines the structure of the User object that we 
// expect to receive in the POST request to the /users endpoint.
// It has two properties: Name, which is a string, and Age, which is an integer.
// a string is a basic data type in most programing languages, 
//and it represents a sequence of characters in double or single quotes.  
//An integer is a whole number, which can be positive, negative, or zero.
record User(string Name, int Age);
