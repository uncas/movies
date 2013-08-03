namespace Uncas.Movies.Web.Crawling
{
    public class CrawledMovie
    {
        public string CrawledMovieId { get; set; }
        public string CrawledMovieUrl { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string ImdbId { get; set; }

        public bool NoImdb()
        {
            return string.IsNullOrWhiteSpace(ImdbId);
        }
    }
}