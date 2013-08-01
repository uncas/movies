using NUnit.Framework;
using Uncas.Movies.Web.Crawling;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class GoogleImdbCrawlerTests
    {
        [Test]
        public void Query_Jagten_Id()
        {
            string imdbId = new GoogleImdbCrawler().QueryImdbId("Jagten");
            Assert.AreEqual("tt2106476", imdbId);
        }

        [Test]
        public void Query_XXXXXXXXXXXXXXXXXXXXXXXXXx_Nothing()
        {
            string imdbId =
                new GoogleImdbCrawler().QueryImdbId("XXXXXXXXXXXXXXXXXXXXXXXXXx");
            Assert.Null(imdbId);
        }
    }
}