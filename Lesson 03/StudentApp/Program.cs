var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var students = new List<Student>
{
    new Student { Id = 1, Name = "Ali", Age = 20, Major = "Computer Science" },
    new Student { Id = 2, Name = "Madina", Age = 21, Major = "Mathematics" },
    new Student { Id = 3, Name = "Jasur", Age = 19, Major = "Physics" }
};

app.MapGet("/", () => "StudentApp ishlayapti");

app.MapGet("/students", () => Results.Ok(students));

app.MapGet("/students/{id:int:min(1)}", (int id) =>
{
    var student = students.FirstOrDefault(s => s.Id == id);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

app.MapPost("/students", (Student student) =>
{
    if (student is null)
    {
        return Results.BadRequest();
    }

    if (student.Id <= 0)
    {
        var nextId = students.Count == 0 ? 1 : students.Max(s => s.Id) + 1;
        student.Id = nextId;
    }
    else if (students.Any(s => s.Id == student.Id))
    {
        return Results.Conflict($"Student with id {student.Id} already exists.");
    }

    students.Add(student);
    return Results.Created($"/students/{student.Id}", student);
});

app.MapPut("/students/{id:int:min(1)}", (int id, Student updated) =>
{
    var student = students.FirstOrDefault(s => s.Id == id);
    if (student is null)
    {
        return Results.NotFound();
    }

    student.Name = updated.Name;
    student.Age = updated.Age;
    student.Major = updated.Major;

    return Results.Ok(student);
});

app.MapDelete("/students/{id:int:min(1)}", (int id) =>
{
    var student = students.FirstOrDefault(s => s.Id == id);
    if (student is null)
    {
        return Results.NotFound();
    }

    students.Remove(student);
    return Results.NoContent();
});

app.Run();
