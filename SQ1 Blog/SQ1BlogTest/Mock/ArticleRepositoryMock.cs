using Microsoft.EntityFrameworkCore;
using Moq;
using SQ1Blog.Entities;
using SQ1Blog.Helpers;
using SQ1Blog.Models;
using SQ1Blog.Services;
using SQ1Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQ1BlogTest.Mock
{
    public class ArticleRepositoryMock : Mock<IArticleRepository>
    {
        public ArticleRepositoryMock GetALLArticles(int count = 0)
        {
            Setup(x => x.GetALLArticles(It.IsAny<int>()))
                .ReturnsAsync(FakeDb.Articles.OrderBy(x => x.PublicationDate).Take(count + 10)
                .Select(y => new ArticleListViewModel
                {
                    Id = y.ID,
                    Title = y.Title,
                    PublicationDate = y.PublicationDate,
                    Description = y.Description,

                }).ToList());

            return this;
        }

        //public ArticleRepositoryMock GetALLArticles_Null()
        //{
        //    Setup(x => x.GetALLArticles(It.IsAny<int>()))
        //        .ReturnsAsync(default(ArticleListViewModel));

        //    return this;
        //}


        public ArticleRepositoryMock GetUserPosts(string userId)
        {
            Setup(x => x.GetALLArticles(It.IsAny<int>()))
                .ReturnsAsync(FakeDb.Articles.Where(a => a.CreatedBy == userId)
                .Select(y => new ArticleListViewModel
                {
                    Id = y.ID,
                    Title = y.Title,
                    PublicationDate = y.PublicationDate,
                    Description = y.Description
                }).ToList());

            return this;
        }
        
        public ArticleRepositoryMock GetArticleById(int Id)
        {
            Setup(x => x.GetArticleById(It.IsAny<int>()))
                .ReturnsAsync(FakeDb.Articles.Where(a => a.ID == Id)
                .Select(y => new ArticleViewModel
                {
                    ID = y.ID,
                    Title = y.Title,
                    PublicationDate = y.PublicationDate,
                    Description = y.Description
                }).FirstOrDefault());

            return this;
        }
        
        public ArticleRepositoryMock AddArticle(Article article)
        {
            var mockSet = new Mock<DbSet<Article>>();

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(m => m.Articles).Returns(mockSet.Object);

            mockSet.Verify(m => m.Add(It.IsAny<Article>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            return this;
        }

    }
}
