using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQ1Blog.Entities;
using SQ1Blog.Helpers;
using SQ1Blog.Models;
using SQ1Blog.Services;
using SQ1Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _articleService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;

        public ArticleController(ILogger<ArticleController> logger, IArticleService articleService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, INotyfService toastnotification)
        {
            _logger = logger;
            _articleService = articleService;
            _signInManager = signInManager;
            _toastNotification = toastnotification;
            _userManager = userManager;
        }

        public async Task<IActionResult> ArticleList()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            

            var data = await _articleService.GetUserPosts(user.Id, user.UserName);

            ArticlePageModel model = new ArticlePageModel
            {
                Articles = data,
                Count = data.Count(),
                IsAdminUser = user.IsAdmin
            };

            return View(model);
        }
        
        public async Task<IActionResult> ImportArticle()
        {
            try
            {
                var data = await _articleService.ImportPosts();

                if (data.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _toastNotification.Success("New Article added successfully!");
                    return RedirectToAction("ArticleList", "Article");
                }
                else
                {
                    _toastNotification.Error("Articles were not imported successfully!");
                    return RedirectToAction("ArticleList", "Article");
                }
            }
            catch(Exception ex)
            {
                _toastNotification.Error("Articles were not imported successfully!");
                return RedirectToAction("ArticleList", "Article");
            }
            


        }
        
        public IActionResult AddArticle()
        {
            return View();
        }



        public async Task<IActionResult> AddArticlePost(AddArticlePageModel article)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _toastNotification.Error("Ensure all details are correctly entered");
                    return RedirectToAction("AddArticleGet", "Article");
                }

                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                article.PublicationDate = DateTime.UtcNow;
                article.CreatedBy = user.Id;

                await _articleService.AddArticle(article);

                _toastNotification.Success("New Article added successfully!");
                return RedirectToAction("ArticleList", "Article");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "" });
            }

        }


    }
}
