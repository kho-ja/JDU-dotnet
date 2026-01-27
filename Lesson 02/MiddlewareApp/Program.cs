var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine("Salom men middlewareman");
    await next();
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware 1: so'rov kelishidan avval");
    await next();
    Console.WriteLine("Middleware 1: javob yuborishidan avval");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware 2: so'rov kelishidan avval");
    await next();
    Console.WriteLine("Middleware 2: javob yuborishidan avval");
});

app.UseMiddleware<RequestHeadersLoggingMiddleware>();

app.MapGet("/", () => "Ism Familiya");

app.Run(async context =>
{
    Console.WriteLine("Terminal middleware: javob qaytaryapman");
    context.Response.ContentType = "text/plain; charset=utf-8";
    await context.Response.WriteAsync("Terminal middleware javobi");
});

app.Run();

class RequestHeadersLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestHeadersLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("Headerlar:");
        foreach (var header in context.Request.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        await _next(context);
    }
}
