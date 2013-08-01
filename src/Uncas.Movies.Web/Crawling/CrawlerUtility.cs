using System.Net.Http;
using System.Threading.Tasks;

namespace Uncas.Movies.Web.Crawling
{
    public static class CrawlerUtility
    {
        public static string Crawl(string url)
        {
            Task<string> task = CrawlAsync(url);
            task.Wait();
            return task.Result;
        }

        private static async Task<string> CrawlAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(url);
                return await message.Content.ReadAsStringAsync();
            }
        }
    }
}