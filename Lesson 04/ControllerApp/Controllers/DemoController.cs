using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace ControllerApp.Controllers;

[Route("demo")]
public class DemoController : Controller
{
    [HttpGet("/hello")]
    public IActionResult Hello([FromQuery] string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Content("Salom");
        }

        return Content($"Salom {name}");
    }

    [HttpGet("/hello/{name}")]
    public IActionResult HelloByName(string name)
    {
        return Content($"Salom {name}");
    }

    [HttpGet("/hello/{name}/{id:int:min(1)}")]
    public IActionResult HelloByNameId(string name, int id)
    {
        return Content($"Salom {name}; id:{id}");
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View("Index", "Controller va Action lar darsi");
    }

    [HttpGet("actions")]
    public IActionResult ActionsList()
    {
        var actions = GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(m => m.DeclaringType == typeof(DemoController))
            .Where(m => typeof(IActionResult).IsAssignableFrom(m.ReturnType))
            .Select(m => m.Name)
            .Distinct()
            .OrderBy(name => name);

        return Json(actions);
    }

    [HttpGet("call-hello/{name}")]
    public IActionResult CallHello(string name)
    {
        var greeting = BuildGreeting(name);
        return Content(greeting);
    }

    [HttpGet("redirect-home")]
    public IActionResult RedirectHome()
    {
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("json")]
    public IActionResult JsonResultDemo()
    {
        return Json(new
        {
            message = "Salom",
            generatedAt = DateTime.Now
        });
    }

    [HttpPost("students")]
    public IActionResult CreateStudent([FromBody] StudentDto? student)
    {
        if (student is null)
        {
            return BadRequest("Student ma'lumotlari yuborilmadi.");
        }

        return Json(new
        {
            action = "create",
            student
        });
    }

    [HttpPut("students/{id:int:min(1)}")]
    public IActionResult UpdateStudent(int id, [FromBody] StudentDto? student)
    {
        if (student is null)
        {
            return BadRequest("Student ma'lumotlari yuborilmadi.");
        }

        return Json(new
        {
            action = "update",
            id,
            student
        });
    }

    [HttpDelete("students/{id:int:min(1)}")]
    public IActionResult DeleteStudent(int id)
    {
        return Json(new
        {
            action = "delete",
            id
        });
    }

    private static string BuildGreeting(string name)
    {
        return $"Salom {name}";
    }

    public class StudentDto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Major { get; set; } = string.Empty;
    }
}
