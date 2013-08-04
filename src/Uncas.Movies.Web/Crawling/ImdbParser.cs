using Newtonsoft.Json;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Crawling
{
    public static class ImdbParser
    {
        public static Movie Parse(string json)
        {
            return JsonConvert.DeserializeObject<Movie>(json);
        }
    }
}