var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hello", (string? name) =>
{
    if (string.IsNullOrWhiteSpace(name))
    {
        return Results.Ok("Salom");
    }

    return Results.Ok($"Salom {name}");
});

app.MapGet("/hello/{name}", (string name) =>
{
    return Results.Ok($"Salom {name}");
});

app.MapGet("/hello/{name}/{id:int:min(1)}", (string name, int id) =>
{
    return Results.Ok($"Salom {name}; id:{id}");
});

app.Run();
