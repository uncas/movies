using System.Collections.Generic;
using System.Linq;

namespace Uncas.Movies.Web.Crawling
{
    public class CrawledMovieRepository
    {
        private static List<CrawledMovie> _crawledMovies;

        public void Save(IEnumerable<CrawledMovie> movies)
        {
            _crawledMovies = movies.ToList();
        }
    }
}