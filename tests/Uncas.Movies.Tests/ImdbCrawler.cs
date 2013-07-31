﻿using Newtonsoft.Json;

namespace Uncas.Movies.Tests
{
    public class ImdbCrawler
    {
        public ImdbMovie CrawlImdb(string imdbId)
        {
            string json =
                CrawlerUtility.Crawl(string.Format("http://www.omdbapi.com/?i={0}", imdbId));
            return JsonConvert.DeserializeObject<ImdbMovie>(json);
        }
    }
}