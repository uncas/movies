using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Crawling
{
    public class ImdbCrawler
    {
        public Movie CrawlImdb(string imdbId)
        {
            string json =
                CrawlerUtility.Crawl(string.Format("http://www.omdbapi.com/?i={0}", imdbId));
            return ImdbParser.Parse(json);
        }
    }
}