using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SQ1Blog.Services;
using SQ1Blog.ViewModels;

namespace SQ1Blog.Controllers
{
    public class SinglePostController : Controller
    {

        private readonly IArticleRepository _articleRepo;

        public SinglePostController(IArticleRepository articleRepository)
        {
            _articleRepo = articleRepository;
        }

        [Route("SinglePost/{id:int}")]
        public async Task<IActionResult> SinglePost(int Id)
        {
            var article = await _articleRepo.GetArticleById(Id);
            ViewBag.Model = article;

            return View();
        }
    }
}