using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModel;

namespace SportsStore.Controllers;

public sealed class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    public ViewResult Login(string returnUrl)
    {
        return View(new LoginModel
        {
            Name = string.Empty,
            Password = string.Empty,
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginModel);
        }
        
        var user = await _userManager.FindByNameAsync(loginModel.Name);
        if (user is not null)
        {
            await _signInManager.SignOutAsync();
                
            var signInResult = await _signInManager.PasswordSignInAsync(
                user, loginModel.Password, false, false);
            if (signInResult.Succeeded)
            {
                return Redirect(loginModel.ReturnUrl ?? "/Admin");
            }
        }
        
        ModelState.AddModelError("", "Invalid name or password");

        return View(loginModel);
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await _signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }
    
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
}