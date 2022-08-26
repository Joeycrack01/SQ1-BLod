using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQ1Blog.Entities;
using SQ1Blog.Models;
using SQ1Blog.ViewModels;

namespace SQ1Blog.Extensions
{
   public static class ArticleExtension { 
      public static void AddToArticle(this Article article, AddArticlePageModel articleVm)
      {
            article.ID = articleVm.ID;
            article.PublicationDate = DateTime.UtcNow;
            article.Title = articleVm.Title;
            article.CreatedBy = articleVm.CreatedBy;
            article.Description = articleVm.Description;
      }

        public static void ConvertToViewModel (this ArticleViewModel articleVm, Article article)
        {
            articleVm.ID = article.ID;
            articleVm.PublicationDate = article.PublicationDate;
            articleVm.Title = article.Title;
            articleVm.Description = article.Description;

        }

    }
}
