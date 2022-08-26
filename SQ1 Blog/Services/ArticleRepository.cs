using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQ1Blog.Extensions;
using Microsoft.EntityFrameworkCore;
using SQ1Blog.ViewModels;
using SQ1Blog.Helpers;
using SQ1Blog.Entities;
using SQ1Blog.Models;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SQ1Blog.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private DataContext _context;

        public ArticleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetALLArticles(int count)
        {
            var articles = await _context.Articles.Include(a => a.User).OrderBy(x => x.PublicationDate).Take(count + 10).ToListAsync();
            return articles;
        }
        
        public async Task<List<Article>> GetALLArticles(string period)
        {
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;

            if (period == "Today")
            {
                dateStart = DateTime.Now.AddHours(1).Date;
                dateEnd = DateTime.Now;
            }
            else
            if (period == "Yesterday")
            {
                dateStart = DateTime.Now.AddHours(1).Date.AddDays(-1);
                dateEnd = DateTime.Now.Date.AddDays(-1).AddHours(24);
            }
            else
            if (period == "Last Week")
            {
                var currentDay = DateTime.Now.DayOfWeek;

                var sundayDate = DateTime.Now.AddDays(-((int)currentDay)).Date;

                dateEnd = sundayDate;
                dateStart = sundayDate.AddDays(-6).Date;

            }
            else
            if (period == "This Week")
            {
                var currentDay = DateTime.Now.DayOfWeek;

                var sundayDate = DateTime.Now.AddDays(-(int)currentDay).Date;

                dateStart = sundayDate;
                dateEnd = sundayDate.Date.AddDays(7).Date;
            }
            else
            if (period == "This Month")
            {
                dateStart = DateTime.Now.AddDays(-(DateTime.Now.Day - 1)).Date;
                dateEnd = DateTime.Now;
            }

            var articles = await _context.Articles.Where(e => e.PublicationDate.Date >= dateStart.Date && e.PublicationDate.Date <= dateEnd.Date).Include(a => a.User).OrderBy(x => x.PublicationDate).ToListAsync();
            return articles;


        }
        
        public async Task<List<Article>> GetUserPosts(string userId)
        {
            var articles = await _context.Articles.Where(a => a.CreatedBy == userId).ToListAsync();
            return articles;
        }

        public async Task<Article> GetArticleById(int Id)
        {

            var article = await _context.Articles.Where(t => t.ID == Id).SingleOrDefaultAsync();
            return article;

        }

        public async Task AddArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }
        
        public async Task AddArticle(List<Article> article)
        {
            _context.Articles.AddRange(article);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteImportedArticle(List<Article> article)
        {
            _context.Articles.RemoveRange(article);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<Article>> GetImportedArticle()
        {
            return await _context.Articles.Where(a => a.IsImported == true).ToListAsync();
        }

        public async Task<ResponseBase<List<ApiArticleResponse>>> ImportPosts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    ///TODO this should go to the appsettings
                    client.BaseAddress = new Uri("https://sq1-api-test.herokuapp.com");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("/posts");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<ServiceResponse>(responseString);

                        return new ResponseBase<List<ApiArticleResponse>>()
                        {
                            Data = res.Data,
                            StatusCode = response.StatusCode,
                            Message = response.StatusCode.ToString()
                        };
                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                        return new ResponseBase<List<ApiArticleResponse>>()
                        {
                            Data = null,
                            StatusCode = response.StatusCode,
                            Message = response.StatusCode.ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<ApiArticleResponse>>()
                {
                    Data = null,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

    }

    public interface IArticleRepository
    {
        Task<Article> GetArticleById(int Id);
        Task<List<Article>> GetALLArticles(int count);
        Task AddArticle(Article articleView);
        Task<List<Article>> GetUserPosts(string Id);
        Task<ResponseBase<List<ApiArticleResponse>>> ImportPosts();
        Task DeleteImportedArticle(List<Article> article);
        Task<List<Article>> GetImportedArticle();
        Task AddArticle(List<Article> article);
        Task<List<Article>> GetALLArticles(string period);
    };
}
