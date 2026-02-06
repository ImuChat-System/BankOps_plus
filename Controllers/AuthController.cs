using BankOpsPlus.Helpers;
using BankOpsPlus.Models.ViewModels;
using BankOpsPlus.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankOpsPlus.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: Auth/Login
    [HttpGet]
    public IActionResult Login()
    {
        // Si déjà authentifié, rediriger vers Dashboard
        if (HttpContext.Session.IsAuthenticated())
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new LoginViewModel());
    }

    // POST: Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.AuthenticateAsync(model.Email, model.Password);

        if (user == null)
        {
            model.ErrorMessage = "Email ou mot de passe incorrect";
            return View(model);
        }

        if (!user.IsActive)
        {
            model.ErrorMessage = "Votre compte est désactivé. Contactez un administrateur.";
            return View(model);
        }

        // Stocker les informations utilisateur en session
        HttpContext.Session.SetUserId(user.Id);
        HttpContext.Session.SetUserName(user.Name);
        HttpContext.Session.SetUserRole(user.Role.ToString());

        return RedirectToAction("Index", "Dashboard");
    }

    // POST: Auth/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.ClearUserSession();
        return RedirectToAction("Login");
    }
}
