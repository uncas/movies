using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Uncas.Movies.Web.Crawling
{
    public static class HtmlParser
    {
        public static IEnumerable<CrawledShow> ExtractShows(string html)
        {
            var crawledShows = new List<CrawledShow>();
            const string dayScheduleSelector =
                "table#top_tabel td.forside_bg table";
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            List<HtmlNode> dayScheduleNodes =
                htmlDocument.DocumentNode.QuerySelectorAll(dayScheduleSelector).ToList();
            foreach (HtmlNode dayScheduleNode in dayScheduleNodes)
            {
                string dayDescription =
                    dayScheduleNode.QuerySelector("tr:nth-child(1) td:nth-child(1)")
                                   .InnerText;
                var dateExpression = new Regex(@"(\d)+[/](\d)+[-](\d)+");
                string dateText = dateExpression.Match(dayDescription).Value;
                DateTime date = DateTime.Parse(dateText, new CultureInfo("da-DK"));
                IEnumerable<HtmlNode> movieNodes =
                    dayScheduleNode.QuerySelectorAll("tr")
                                   .Where((item, index) => index > 0 && index%2 == 0);
                foreach (HtmlNode movieNode in movieNodes)
                {
                    HtmlNode titleNode = movieNode.QuerySelector("td:nth-child(1)");
                    HtmlNode linkNode = titleNode.QuerySelector("a");
                    string title = linkNode.InnerText;
                    string movieUrl = linkNode.Attributes["href"].Value;

                    var idExpression = new Regex(@"(\d)+");
                    string id = idExpression.Match(movieUrl).Value;

                    HtmlNode timeNode = movieNode.QuerySelector("td:nth-child(3)");
                    string timeText = timeNode.InnerText;
                    var timeExpression = new Regex(@"(\d)+[:](\d)+");
                    string time = timeExpression.Match(timeText).Value;

                    HtmlNode showLinkNode = timeNode.QuerySelector("a");
                    string showUrl = showLinkNode.Attributes["href"].Value;

                    string[] timeParts = time.Split(':');
                    int hours = int.Parse(timeParts[0]);
                    int minutes = int.Parse(timeParts[1]);

                    crawledShows.Add(new CrawledShow
                        {
                            ShowTime = date.AddHours(hours).AddMinutes(minutes),
                            CrawledMovieId = id,
                            CrawledMovieUrl = movieUrl,
                            ShowTitle = title,
                            CrawledShowUrl = showUrl
                        });
                }
            }

            return crawledShows;
        }
    }
}