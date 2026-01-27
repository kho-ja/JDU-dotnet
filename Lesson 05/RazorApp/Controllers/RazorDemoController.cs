using Microsoft.AspNetCore.Mvc;
using RazorApp.Models;

namespace RazorApp.Controllers;

public class RazorDemoController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = BuildModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Submit(StudentForm form)
    {
        var model = BuildModel();
        model.Form = form;
        ViewData["Message"] = "Forma yuborildi";
        return View("Index", model);
    }

    private static RazorDemoViewModel BuildModel()
    {
        return new RazorDemoViewModel
        {
            Title = "Razor sintaksisi va View-lar",
            GeneratedAt = DateTime.Now,
            VisitsToday = 12,
            IsLoggedIn = true,
            Students = new List<Student>
            {
                new Student { Name = "Ali", Age = 20, Major = "Computer Science" },
                new Student { Name = "Madina", Age = 21, Major = "Mathematics" },
                new Student { Name = "Jasur", Age = 19, Major = "Physics" }
            },
            Form = new StudentForm()
        };
    }
}
