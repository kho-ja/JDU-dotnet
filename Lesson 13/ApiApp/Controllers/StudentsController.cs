using ApiApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private static readonly List<StudentDto> Students = new()
    {
        new StudentDto { Id = 1, Name = "Ali", Age = 20, Major = "Computer Science" },
        new StudentDto { Id = 2, Name = "Madina", Age = 21, Major = "Mathematics" }
    };

    [HttpGet]
    public ActionResult<IEnumerable<StudentDto>> GetAll([FromQuery] string? search, [FromQuery] string? major)
    {
        var query = Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(major))
        {
            query = query.Where(s => s.Major.Equals(major, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<StudentDto> GetById(int id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    public ActionResult<StudentDto> Create([FromBody] StudentDto student)
    {
        var nextId = Students.Count == 0 ? 1 : Students.Max(s => s.Id) + 1;
        student.Id = nextId;
        Students.Add(student);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPut("{id:int}")]
    public ActionResult<StudentDto> Update(int id, [FromBody] StudentDto updated)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        student.Name = updated.Name;
        student.Age = updated.Age;
        student.Major = updated.Major;
        return Ok(student);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        Students.Remove(student);
        return NoContent();
    }
}
