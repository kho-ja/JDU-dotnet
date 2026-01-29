using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthApp.Controllers;

public class AuthDemoController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthDemoController> _logger;

    public AuthDemoController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration config,
        ILogger<AuthDemoController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult CookieProtected()
    {
        _logger.LogInformation("CookieProtected accessed by {User}", User.Identity?.Name);
        return Content($"Cookie orqali autentifikatsiya: {User.Identity?.Name}");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult AdminOnly()
    {
        return Content("Admin role bilan kirildi.");
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public IActionResult JwtProtected()
    {
        var name = User.Identity?.Name ?? "unknown";
        return Content($"JWT orqali autentifikatsiya: {name}");
    }

    [Authorize]
    [HttpGet]
    public IActionResult SessionDemo()
    {
        var count = HttpContext.Session.GetInt32("count") ?? 0;
        count++;
        HttpContext.Session.SetInt32("count", count);
        return Content($"Session count: {count}");
    }

    [HttpPost]
    public async Task<IActionResult> JwtLogin([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email va parol talab qilinadi.");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var token = await CreateJwtAsync(user);
        return Ok(new { token });
    }

    private async Task<string> CreateJwtAsync(IdentityUser user)
    {
        var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured.");
        var issuer = _config["Jwt:Issuer"] ?? "AuthApp";
        var audience = _config["Jwt:Audience"] ?? "AuthAppUsers";
        var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var minutes) ? minutes : 60;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.Email ?? user.UserName ?? "user")
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public record LoginRequest(string Email, string Password);
}
