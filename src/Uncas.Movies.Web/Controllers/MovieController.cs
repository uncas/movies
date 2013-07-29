using System.Collections.Generic;
using System.Web.Http;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Controllers
{
    public class MovieController : ApiController
    {
        // GET api/movie
        public IEnumerable<MovieDto> GetMovies()
        {
            return new[]
                {
                    new MovieDto
                        {
                            MovieId = 1,
                            Title = "Red",
                            ImdbUrl = "http://www.imdb.com/title/tt0111495/",
                            ShowUrl = "http://www.paradisbio.dk"
                        },
                    new MovieDto {MovieId = 2, Title = "Blue"},
                    new MovieDto {MovieId = 3, Title = "White"}
                };
        }
    }
}