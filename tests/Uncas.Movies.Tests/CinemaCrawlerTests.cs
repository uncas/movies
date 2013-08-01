using NUnit.Framework;

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