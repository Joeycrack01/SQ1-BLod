using SQ1Blog.Entities;
using SQ1Blog.Extensions;
using SQ1Blog.Helpers;
using SQ1Blog.Models;
using SQ1Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Services
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepository;
        private IUserRepository _userRepository;
        public ArticleService(IArticleRepository articleRepository, IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        public async Task<List<ArticleListViewModel>> GetALLArticles(int count, string period)
        {
            List<Article> articles = new List<Article>();
            if (string.IsNullOrEmpty(period))
            {
                articles = await _articleRepository.GetALLArticles(count);
            }
            else
            {
                articles = await _articleRepository.GetALLArticles(period);
            }
            

            var articleVm = articles.Select(y => new ArticleListViewModel
            {
                Id = y.ID,
                Title = y.Title,
                PublicationDate = y.PublicationDate,
                Description = y.Description,
                Author =  y.User.UserName
            }).ToList();
            return articleVm;
        }

        public async Task<List<ArticleListViewModel>> GetUserPosts(string userId, string userName)
        {
            var articles = await _articleRepository.GetUserPosts(userId);
            var articleVm = articles.Select(y => new ArticleListViewModel
            {
                Id = y.ID,
                Title = y.Title,
                PublicationDate = y.PublicationDate,
                Description = y.Description,
                UserId = y.CreatedBy,
                Author = userName
            }).ToList();
            return articleVm;
        }
        
        public async Task<ResponseBase<List<ApiArticleResponse>>> ImportPosts()
        {

            var adminUser = await _userRepository.GetAdminUser();
            var result = await _articleRepository.ImportPosts();

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var article = result.Data.Select(e => new Article()
                {
                    CreatedBy = adminUser.Id,
                    Description = e.Description,
                    PublicationDate = e.PublicaionDate,
                    Title = e.Title,
                    IsImported = true,
                }).ToList();

                var importedArticles = await _articleRepository.GetImportedArticle();
                
                //if (importedArticles.Count > 0)
                //    await _articleRepository.DeleteImportedArticle(importedArticles);

                await _articleRepository.AddArticle(article);

                return new ResponseBase<List<ApiArticleResponse>>()
                {
                    Data = null,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };

            }
            else
            {
                return result;
            }
        }

        public async Task<ArticleViewModel> GetArticleById(int Id)
        {
            var article = await _articleRepository.GetArticleById(Id);
            if (article == null)
            {
                return null;
            }
            else
            {
                var articleVm = new ArticleViewModel
                {
                    PublicationDate = article.PublicationDate,
                    ID = article.ID,
                    Title = article.Title,
                    Description = article.Description
                };


                return articleVm;
            }

        }

        public async Task AddArticle(AddArticlePageModel articleView)
        {
            Article article = new Article();
            article.AddToArticle(articleView);
            await _articleRepository.AddArticle(article);
        }

    }

    public interface IArticleService
    {
        Task<ArticleViewModel> GetArticleById(int Id);
        Task<List<ArticleListViewModel>> GetALLArticles(int count, string period);
        Task AddArticle(AddArticlePageModel articleView);
        Task<List<ArticleListViewModel>> GetUserPosts(string Id, string userName);
        Task<ResponseBase<List<ApiArticleResponse>>> ImportPosts();
    };
}
