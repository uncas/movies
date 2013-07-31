using System;

namespace Uncas.Movies.Tests
{
    public class CrawledShow
    {
        public DateTime ShowTime { get; set; }
        public string CrawledMovieId { get; set; }
        public string CrawledMovieUrl { get; set; }
        public string ShowTitle { get; set; }
        public string ImdbId { get; set; }
    }
}