using SQ1Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Models
{
    public class ArticlePageModel
    {
        public List<ArticleListViewModel> Articles { get; set; }
        public int Count { get; set; }
        public bool IsAdminUser { get; set; }
    }
}
