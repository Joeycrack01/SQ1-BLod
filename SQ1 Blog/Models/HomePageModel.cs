using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SQ1Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Models
{
    public class HomePageModel
    {
        [BindProperty]
        public List<ArticleListViewModel> AllArticles { get; set; }
        [BindProperty]
        public int Count { get; set; }
    }
}
