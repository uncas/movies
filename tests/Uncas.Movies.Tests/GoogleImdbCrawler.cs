using System;
using System.Linq;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Uncas.Movies.Tests
{
    public class GoogleImdbCrawler
    {
        public string QueryImdbId(string query)
        {
            string url = string.Format(
                "https://www.google.dk/search?q={0}+site%3Aimdb.com",
                query);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(CrawlerUtility.Crawl(url));
            const string selector = "div#search li.g:nth-child(1)";
            HtmlNode resultNodes = htmlDocument.DocumentNode.QuerySelector(selector);
            HtmlNode citeNode;
            try
            {
                citeNode = resultNodes.QuerySelectorAll("cite").FirstOrDefault();
            }
            catch (NullReferenceException e)
            {
                // Since there is an error inside the Fizzler library.
                return null;
            }
            if (citeNode == null)
                return null;
            string imdbUrl = citeNode.InnerText;
            return ExtractImdbId(imdbUrl);
        }

        public static string ExtractImdbId(string imdbUrl)
        {
            return new Regex(@"[t][t](\d)+").Match(imdbUrl).Value;
        }
    }
}