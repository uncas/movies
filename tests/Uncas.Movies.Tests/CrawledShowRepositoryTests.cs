using System;
using NUnit.Framework;
using Uncas.Movies.Web.Crawling;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class CrawledShowRepositoryTests
    {
        [Test]
        public void Save_X()
        {
            var repository = new CrawledShowRepository();
            repository.Save(new[]
                {
                    new CrawledShow
                        {
                            CrawledMovieId = "ABC",
                            CrawledMovieUrl = "http",
                            CinemaId = 1,
                            ShowTime = DateTime.Now,
                            ShowTitle = "Red"
                        }
                });
        }
    }
}