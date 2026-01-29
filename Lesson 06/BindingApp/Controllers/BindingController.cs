using Microsoft.AspNetCore.Mvc;
using BindingApp.Models;

namespace BindingApp.Controllers;

public class BindingController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var sample = new Student
        {
            Name = "Ali",
            Age = 20,
            Major = "Computer Science",
            Email = "ali@example.com"
        };

        return View(sample);
    }

    [HttpPost]
    public IActionResult Index(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        return View("Result", student);
    }

    [HttpGet]
    public IActionResult Result(Student student)
    {
        return View(student);
    }
}
