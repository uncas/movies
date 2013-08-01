using NUnit.Framework;
using Uncas.Movies.Web.Crawling;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class CinemaCrawlerTests
    {
        [Test]
        public void CrawlEastOfEden()
        {
            new CinemaCrawler().CrawlCinema();
        }
    }
}