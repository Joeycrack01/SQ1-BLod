using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQ1Blog.Entities;
using SQ1Blog.Models;
using SQ1Blog.Services;

namespace SQ1Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, IArticleService articleService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _articleService = articleService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index(string period, int? Id = 0)
        {
            try
            {
                HomePageModel homePageModel = new HomePageModel();

                var allArticles = await _articleService.GetALLArticles((int)Id, period);

                homePageModel.AllArticles = allArticles;

                ViewBag.Model = homePageModel;

                return View(homePageModel);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
