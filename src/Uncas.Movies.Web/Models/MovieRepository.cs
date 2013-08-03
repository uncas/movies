using System.Collections.Generic;
using System.Linq;

namespace Uncas.Movies.Web.Models
{
    internal class MovieRepository
    {
        private static IList<Movie> _movies;

        public void Save(IEnumerable<Movie> movies)
        {
            _movies = movies.ToList();
        }
    }
}