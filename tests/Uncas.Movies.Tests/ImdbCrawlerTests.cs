using NUnit.Framework;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class ImdbCrawlerTests
    {
        private static ImdbMovie CrawlImdb(string imdbId)
        {
            return new ImdbCrawler().CrawlImdb(imdbId);
        }

        [Test]
        public void CrawlImdb()
        {
            const string imdbId = "tt2106476";

            ImdbMovie movie = CrawlImdb(imdbId);

            Assert.AreEqual("The Hunt", movie.Title);
            Assert.AreEqual(2012, movie.Year);
            Assert.AreEqual(8.2d, movie.ImdbRating);
            Assert.AreEqual(
                "http://ia.media-imdb.com/images/M/MV5BMTg2NDg3ODg4NF5BMl5BanBnXkFtZTcwNzk3NTc3Nw@@._V1_SX300.jpg",
                movie.Poster);
            Assert.AreEqual(imdbId, movie.ImdbId);
        }
    }
}