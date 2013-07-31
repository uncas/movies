using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class CinemaCrawlerTests
    {
        private async Task<string> CrawlAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(url);
                return await message.Content.ReadAsStringAsync();
            }
        }

        private string Crawl(string url)
        {
            Task<string> task = CrawlAsync(url);
            task.Wait();
            return task.Result;
        }

        [Test]
        public void CrawlEastOfEden()
        {
            string html = Crawl("http://www.paradisbio.dk/program.asp?bio=aarhusc");
            const string dayScheduleSelector =
                "table#top_tabel td.forside_bg table";
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            List<HtmlNode> dayScheduleNodes =
                htmlDocument.DocumentNode.QuerySelectorAll(dayScheduleSelector).ToList();
            Console.WriteLine("{0} day schedules", dayScheduleNodes.Count);
            foreach (HtmlNode dayScheduleNode in dayScheduleNodes)
            {
                string dayDescription =
                    dayScheduleNode.QuerySelector("tr:nth-child(1) td:nth-child(1)")
                                   .InnerText;
                Console.WriteLine(dayDescription);
                IEnumerable<HtmlNode> movieNodes =
                    dayScheduleNode.QuerySelectorAll("tr")
                                   .Where((item, index) => index > 0 && index%2 == 0);
                foreach (HtmlNode movieNode in movieNodes)
                {
                    HtmlNode titleNode = movieNode.QuerySelector("td:nth-child(1)");
                    string title = titleNode.InnerText;
                    string link = titleNode.QuerySelector("a").Attributes["href"].Value;
                    string time = movieNode.QuerySelector("td:nth-child(3)").InnerText;
                    Console.WriteLine("{0} - {1} - {2}", time, title, link);
                }
            }

            // TODO: Take only the following 3 days.
            // TODO: Save the list of shows.
            // TODO: Lookup more details for unique ids that are still not recorded in our system.
            // TODO: Run once per day.
        }
    }
}