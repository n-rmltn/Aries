using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Aries.Models;
public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                TempData["ToastTitle"] = "Success";
                TempData["ToastMessage"] = "Logged in successfully!";
                return RedirectToAction("Index", "Home");
            }
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Invalid login attempt.";
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignupViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["ToastTitle"] = "Success"; 
                TempData["ToastMessage"] = "Account created successfully!";
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["ToastTitle"] = "Success";
        TempData["ToastMessage"] = "Logged out successfully!";
        return RedirectToAction("Index", "Home");
    }
}