using EfCrudApp.Data;
using EfCrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCrudApp.Controllers;

public class StudentsController : Controller
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, string? major, int? minAge, int? maxAge, string? sort, string? order)
    {
        var query = _db.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.Name.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(major))
        {
            query = query.Where(s => s.Major == major);
        }

        if (minAge.HasValue)
        {
            query = query.Where(s => s.Age >= minAge.Value);
        }

        if (maxAge.HasValue)
        {
            query = query.Where(s => s.Age <= maxAge.Value);
        }

        var isDesc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
        query = (sort ?? "id").ToLowerInvariant() switch
        {
            "name" => isDesc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
            "age" => isDesc ? query.OrderByDescending(s => s.Age) : query.OrderBy(s => s.Age),
            "major" => isDesc ? query.OrderByDescending(s => s.Major) : query.OrderBy(s => s.Major),
            _ => isDesc ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
        };

        ViewData["Search"] = search;
        ViewData["Major"] = major;
        ViewData["MinAge"] = minAge;
        ViewData["MaxAge"] = maxAge;
        ViewData["Sort"] = sort ?? "id";
        ViewData["Order"] = order ?? "asc";

        var students = await query.ToListAsync();
        return View(students);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Student());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        _db.Students.Add(student);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(student);
        }

        _db.Students.Update(student);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
