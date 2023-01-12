using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Tourfirm.DAL;
using Tourfirm.DAL.ViewModels;
using Tourfirm.Service.Implementations;

namespace Tourfirm.Controllers;

public class AuthController : Controller
{
    private readonly ApplicationContext db;
    private readonly IWebHostEnvironment _app;
    private readonly IAuthService _authService;

    private readonly ILogger<AuthController> _logger;
    
    public AuthController(ApplicationContext context, IWebHostEnvironment app, IAuthService authService)
    {
        db = context;
        _app = app;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();
    
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _authService.Login(model);
            if (response.StatusCode == Domain.Safety.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                return RedirectToAction("Main", "Home");
            }
            ModelState.AddModelError("", response.Description);
        }
        return View(model);
    }
    
    [HttpGet]
    public IActionResult Register() => View();
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(ModelState.IsValid)
        {
            var response = await _authService.Register(model);
            if (response.StatusCode == Domain.Safety.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                return RedirectToAction("Login", "Auth");
            }
            ModelState.AddModelError("", response.Description);
        }
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");
    }

}