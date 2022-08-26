using NSubstitute;
using Shouldly;
using SQ1Blog.Entities;
using SQ1Blog.Helpers;
using SQ1Blog.Services;
using SQ1Blog.ViewModels;
using SQ1BlogTest.Mock;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SQ1BlogTest
{
    public class ArticleServiceTest
    {
        private readonly IArticleRepository _articleRepo;
        private readonly IUserRepository _userRepository;
        public ArticleServiceTest()
        {
            _articleRepo = Substitute.For<IArticleRepository>();
            _userRepository = Substitute.For<IUserRepository>();
        }
        private ArticleService Sut => new ArticleService(_articleRepo, _userRepository);

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetArticleById_ReturnsArticle(int Id)
        {
            _articleRepo.GetArticleById(Id).Returns(new Article() { ID = Id, CreatedBy = "sdsdsdsd", Description = "aasasa", PublicationDate = DateTime.Now, Title = "Test"});

            var result = await Sut.GetArticleById(Id);

            Assert.NotNull(result);
            Assert.Equal(result.ID, Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetArticleById_ReturnsNull(int Id)
        {
            var article = await GetNull();
            _articleRepo.GetArticleById(Id).Returns(article);

            var result = await Sut.GetArticleById(Id);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(10, "")]
        [InlineData(1, "")]
        public async Task GetALLArticles_ReturnsArticleList(int count, string period)
        {
            var articleList = GetArticleList(count);
            _articleRepo.GetALLArticles(count).Returns(articleList);

            var result = await Sut.GetALLArticles(count, period);

            result.Count.ShouldBeGreaterThan(0);

            if (count == 0)
            {
                result.Count.ShouldBe(10);
            }

            if (count == 10)
            {
                result.Count.ShouldBe(20);
            }
        }

        [Theory]
        [InlineData(10, "")]
        public async Task GetALLArticles_ReturnsZeroArticles(int count, string period)
        {
            var articleList = GetZeroCountArticleList(count);
            _articleRepo.GetALLArticles(count).Returns(articleList);

            var result = await Sut.GetALLArticles(count, period);

            result.Count.ShouldBe(0);

        }

        [Theory]
        [InlineData("75950c57-6691-427d-8f3b-1c4ad58239a8", "admin")]
        [InlineData("75950c57-6691-427d-8f3b-1c4ad58239a7", "admin")]
        [InlineData("75950c57-6691-427d-8f3b-1c4ad58239a6", "admin")]
        [InlineData("75950c57-6691-427d-8f3b-1c4ad58239a5", "admin")]
        [InlineData("75950c57-6691-427d-8f3b-1c4ad58239a4", "admin")]
        public async Task GetALLArticles_ReturnsUserArticleList(string userId, string username)
        {
            var articleList = GetUserArticleList(userId);
            _articleRepo.GetUserPosts(userId).Returns(articleList);

            var result = await Sut.GetUserPosts(userId, username);

            result.ShouldContain(a => a.UserId == userId && a.Author == username);
        }

        private async Task<Article> GetNull ()
        {
            return null;
        }

        private List<Article> GetArticleList(int count)
        {
            FakeDb fakeDb = new FakeDb();
            return fakeDb.GetArticleList(count);
        }

        private List<Article> GetZeroCountArticleList(int count)
        {
            return new List<Article>();
        }

        private List<Article> GetUserArticleList(string userId)
        {
            FakeDb fakeDb = new FakeDb();
            return fakeDb.GetUserArticleList(userId);
        }
    }
}
