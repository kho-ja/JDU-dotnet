using Microsoft.AspNetCore.Mvc;
using ValidationApp.Models;

namespace ValidationApp.Controllers;

public class ValidationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = new StudentRegistration
        {
            Name = "Ali",
            Surname = "Karimov",
            Age = 20,
            Major = "Computer Science",
            Email = "ali@example.com",
            StudentId = "ST-1234"
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(StudentRegistration model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return View("Result", model);
    }
}
