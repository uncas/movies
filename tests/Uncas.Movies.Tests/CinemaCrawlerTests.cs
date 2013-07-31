using System;
using System.Net.Http;
using System.Threading.Tasks;
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
            string message = Crawl("http://www.paradisbio.dk/program.asp?bio=aarhusc");
            Console.WriteLine(message);
        }
    }
}