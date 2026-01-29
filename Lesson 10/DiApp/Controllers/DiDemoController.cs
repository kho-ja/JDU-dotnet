using DiApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiApp.Controllers;

public class DiDemoController : Controller
{
    private readonly IOperationService _operations;
    private readonly RequestTracker _tracker;
    private readonly ILogger<DiDemoController> _logger;
    private readonly IConfiguration _config;

    public DiDemoController(
        IOperationService operations,
        RequestTracker tracker,
        ILogger<DiDemoController> logger,
        IConfiguration config)
    {
        _operations = operations;
        _tracker = tracker;
        _logger = logger;
        _config = config;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var greeting = _config["AppSettings:Greeting"] ?? "Salom!";
        _logger.LogInformation("DI Demo ishlamoqda. TrackerId={TrackerId}", _tracker.Id);

        var model = new Dictionary<string, string>
        {
            ["Greeting"] = greeting,
            ["Singleton"] = _operations.SingletonId.ToString(),
            ["Scoped"] = _operations.ScopedId.ToString(),
            ["Transient"] = _operations.TransientId.ToString(),
            ["RequestTracker"] = _tracker.Id.ToString()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Second()
    {
        _logger.LogInformation("Second action. TrackerId={TrackerId}", _tracker.Id);

        var model = new Dictionary<string, string>
        {
            ["Singleton"] = _operations.SingletonId.ToString(),
            ["Scoped"] = _operations.ScopedId.ToString(),
            ["Transient"] = _operations.TransientId.ToString(),
            ["RequestTracker"] = _tracker.Id.ToString()
        };

        return View("Index", model);
    }
}
