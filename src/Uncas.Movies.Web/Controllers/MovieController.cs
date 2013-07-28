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
                    new MovieDto {MovieId = 1, Title = "Red"},
                    new MovieDto {MovieId = 2, Title = "Blue"},
                    new MovieDto {MovieId = 3, Title = "White"}
                };
        }
    }
}