using System;

namespace Uncas.Movies.Web.Crawling
{
    public class CrawledShow
    {
        public int CinemaId { get; set; }
        public DateTime ShowTime { get; set; }
        public string CrawledMovieId { get; set; }
        public string CrawledMovieUrl { get; set; }
        public string ShowTitle { get; set; }
        public string CrawledShowUrl { get; set; }
    }
}