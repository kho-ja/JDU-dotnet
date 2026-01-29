using DebugApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DebugApp.Controllers;

public class DiagnosticsController : Controller
{
    private readonly CalculatorService _calculator;
    private readonly ILogger<DiagnosticsController> _logger;

    public DiagnosticsController(CalculatorService calculator, ILogger<DiagnosticsController> logger)
    {
        _calculator = calculator;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Add(int a = 2, int b = 3)
    {
        var result = _calculator.Add(a, b);
        _logger.LogInformation("Add ishladi: {A} + {B} = {Result}", a, b, result);
        return Ok(new { a, b, result });
    }

    [HttpGet]
    public IActionResult Divide(int a = 10, int b = 2)
    {
        try
        {
            var result = _calculator.Divide(a, b);
            return Ok(new { a, b, result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Divide xatosi: {A} / {B}", a, b);
            return Problem(detail: ex.Message, statusCode: 500);
        }
    }

    [HttpGet]
    public IActionResult Throw()
    {
        throw new InvalidOperationException("Bu debugging uchun test exception.");
    }
}
