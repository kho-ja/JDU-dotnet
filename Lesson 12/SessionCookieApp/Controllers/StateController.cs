using Microsoft.AspNetCore.Mvc;

namespace SessionCookieApp.Controllers;

public class StateController : Controller
{
    private const string SessionKeyCounter = "counter";
    private const string CookieUserId = "user_id";
    private const string CookieUserName = "user_name";

    [HttpGet]
    public IActionResult Index()
    {
        var counter = HttpContext.Session.GetInt32(SessionKeyCounter) ?? 0;
        var userId = Request.Cookies[CookieUserId] ?? "anonymous";
        var userName = Request.Cookies[CookieUserName] ?? "Mehmon";

        ViewData["Counter"] = counter;
        ViewData["UserId"] = userId;
        ViewData["UserName"] = userName;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Increment()
    {
        var counter = HttpContext.Session.GetInt32(SessionKeyCounter) ?? 0;
        counter++;
        HttpContext.Session.SetInt32(SessionKeyCounter, counter);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetSession()
    {
        HttpContext.Session.Remove(SessionKeyCounter);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetCookie(string userName)
    {
        var userId = Request.Cookies[CookieUserId];
        if (string.IsNullOrWhiteSpace(userId))
        {
            userId = Guid.NewGuid().ToString("N");
        }

        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            IsEssential = true
        };

        Response.Cookies.Append(CookieUserId, userId, options);
        Response.Cookies.Append(CookieUserName, userName, options);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearCookies()
    {
        Response.Cookies.Delete(CookieUserId);
        Response.Cookies.Delete(CookieUserName);
        return RedirectToAction(nameof(Index));
    }
}
