using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SQ1Blog.Entities;
using SQ1Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _externalCookieScheme;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastnotification;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityConstants> identityCookieOptions,
            ILogger<AccountController> logger,
            INotyfService toastnotification,
            UserManager<ApplicationUser> userManager
            )
        {
            _signInManager = signInManager;
            _externalCookieScheme = IdentityConstants.ApplicationScheme;
            _logger = logger;
            _toastnotification = toastnotification;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> LoginPost(LoginPageModel loginPageModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginPageModel.UserName);

                var result = await _signInManager.PasswordSignInAsync(loginPageModel.UserName, loginPageModel.Password, false, false);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    _toastnotification.Success($"{loginPageModel.UserName} you've logged in successfully");
                    _logger.LogInformation(1, "User logged in.");
                    if (string.IsNullOrEmpty(loginPageModel.ReturnUrl) || loginPageModel.ReturnUrl == "/Account/Logout")
                        return RedirectToAction("Index", "Home", new { area = "" });

                    return Redirect(loginPageModel.ReturnUrl);

                }
               
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    _toastnotification.Error("Invalid login attempt.");
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
            }

            // If we got this far, something failed, redisplay form
            _toastnotification.Warning("Login was unsuccessfull please try again or contact an administrator");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> RegisterPost(RegistrationPageModel RegisterPageModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = RegisterPageModel.UserName, Email = RegisterPageModel.Email };
                var result = await _userManager.CreateAsync(user, RegisterPageModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    user.LockoutEnabled = false;
                    _toastnotification.Success($"Welcome {RegisterPageModel.UserName}! <br/> account created successfully");

                   return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _toastnotification.Error(error.Description);
                }
            }

            return RedirectToAction("Register", "Account", new { area = "" });
        }
    }
}
